// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Metalama.Framework.Aspects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Metalama.Community.AutoCancellationToken
{
    internal partial class AutoCancellationTokenWeaver
    {
        [CompileTime]
        private sealed class AddCancellationTokenParameterRewriter : RewriterBase
        {
            private readonly Compilation _compilation;
            private readonly SyntaxAnnotation _generatedCodeAnnotation;

            public AddCancellationTokenParameterRewriter(
                Compilation compilation,
                SyntaxAnnotation generatedCodeAnnotation )
            {
                this._compilation = compilation;
                this._generatedCodeAnnotation = generatedCodeAnnotation;
            }

            protected override T VisitTypeDeclaration<T>( T node, Func<T, SyntaxNode?> baseVisit )
            {
                if ( !node.HasAnnotation( AnnotateNodesRewriter.Annotation ) )
                {
                    return node;
                }

                return (T) baseVisit( node )!;
            }

            public override SyntaxNode VisitMethodDeclaration( MethodDeclarationSyntax node )
            {
                var semanticModel = this._compilation.GetSemanticModel( node.SyntaxTree );

                var methodSymbol = semanticModel.GetDeclaredSymbol( node );

                if ( methodSymbol is not { IsAsync: true } ||
                     methodSymbol.Parameters.Any( IsCancellationToken ) ||
                     methodSymbol.Parameters.LastOrDefault() is { IsParams: true } )
                {
                    return node;
                }

                // Widening the signature of a method that overrides or implements another member would sever that
                // relationship and produce code that does not compile.
                if ( methodSymbol.IsOverride ||
                     !methodSymbol.ExplicitInterfaceImplementations.IsDefaultOrEmpty ||
                     ImplementsInterfaceMember( methodSymbol ) )
                {
                    return node;
                }

                // Adding the parameter must not produce a duplicate of a method that already exists in the type.
                if ( WouldCollideWithExistingMember( methodSymbol ) )
                {
                    return node;
                }

                // TODO: Review: finding a unique name is a common pattern. Is there a common library implementation?
                const string defaultParameterName = "cancellationToken";
                var useParameterName = defaultParameterName;

                // The name must not collide with a parameter, a type parameter, or anything declared in the body:
                // a local declared in an enclosing scope of a nested one causes CS0136, and reusing the name of an
                // existing local would silently pass that local instead of the token. Collecting every identifier in
                // the declaration is a deliberate over-approximation - it can only make us pick a longer name.
                var usedNames = new HashSet<string>( StringComparer.Ordinal );

                foreach ( var token in node.DescendantTokens() )
                {
                    if ( token.IsKind( SyntaxKind.IdentifierToken ) )
                    {
                        usedNames.Add( token.ValueText );
                    }
                }

                for ( var i = 2; usedNames.Contains( useParameterName ); ++i )
                {
                    useParameterName = $"{defaultParameterName}{i}";
                }

                var parameters = node.ParameterList.Parameters.GetWithSeparators().ToList();

                if ( parameters.Count > 0 )
                {
                    // Remove the trivia after the last argument.
                    parameters[parameters.Count - 1] =
                        parameters[parameters.Count - 1].AsNode()!.WithoutTrailingTrivia();

                    parameters.Add(
                        SyntaxFactory.Token( SyntaxKind.CommaToken )
                            .WithTrailingTrivia( SyntaxFactory.ElasticSpace )
                            .WithAdditionalAnnotations( this._generatedCodeAnnotation ) );
                }

                parameters.Add(
                    SyntaxFactory.Parameter(
                            default,
                            default,
                            CancellationTokenType.WithTrailingTrivia( SyntaxFactory.ElasticSpace ),
                            SyntaxFactory.Identifier( useParameterName )
                                .WithTrailingTrivia( SyntaxFactory.ElasticSpace ),
                            SyntaxFactory.EqualsValueClause(
                                    SyntaxFactory.Token( SyntaxKind.EqualsToken )
                                        .WithTrailingTrivia( SyntaxFactory.ElasticSpace ),
                                    SyntaxFactory.LiteralExpression( SyntaxKind.DefaultLiteralExpression ) )
                                .WithTrailingTrivia( SyntaxFactory.ElasticSpace ) )
                        .WithTrailingTrivia( SyntaxFactory.ElasticSpace )
                        .WithAdditionalAnnotations( this._generatedCodeAnnotation ) );

                node = node.WithParameterList(
                    SyntaxFactory.ParameterList( SyntaxFactory.SeparatedList<ParameterSyntax>( [..parameters] ) ) );

                return node;
            }

            /// <summary>
            /// Determines whether <paramref name="method"/> implicitly implements a member of an interface implemented
            /// by its containing type. Explicit implementations are reported by <see cref="ISymbol.ExplicitInterfaceImplementations"/>
            /// and are checked separately.
            /// </summary>
            private static bool ImplementsInterfaceMember( IMethodSymbol method )
            {
                var containingType = method.ContainingType;

                if ( containingType == null )
                {
                    return false;
                }

                foreach ( var interfaceType in containingType.AllInterfaces )
                {
                    foreach ( var interfaceMember in interfaceType.GetMembers( method.Name ) )
                    {
                        if ( interfaceMember is IMethodSymbol &&
                             SymbolEqualityComparer.Default.Equals(
                                 containingType.FindImplementationForInterfaceMember( interfaceMember ),
                                 method ) )
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            /// <summary>
            /// Determines whether adding a trailing <see cref="System.Threading.CancellationToken"/> parameter to
            /// <paramref name="method"/> would produce a signature that another member of the same type already has.
            /// </summary>
            private static bool WouldCollideWithExistingMember( IMethodSymbol method )
            {
                var containingType = method.ContainingType;

                if ( containingType == null )
                {
                    return false;
                }

                foreach ( var member in containingType.GetMembers( method.Name ) )
                {
                    if ( member is not IMethodSymbol otherMethod ||
                         SymbolEqualityComparer.Default.Equals( otherMethod, method ) ||
                         otherMethod.Parameters.Length != method.Parameters.Length + 1 ||
                         otherMethod.TypeParameters.Length != method.TypeParameters.Length )
                    {
                        continue;
                    }

                    if ( !IsCancellationToken( otherMethod.Parameters[otherMethod.Parameters.Length - 1] ) )
                    {
                        continue;
                    }

                    var parametersMatch = true;

                    for ( var i = 0; i < method.Parameters.Length; i++ )
                    {
                        if ( !SymbolEqualityComparer.Default.Equals(
                                otherMethod.Parameters[i].Type,
                                method.Parameters[i].Type ) )
                        {
                            parametersMatch = false;

                            break;
                        }
                    }

                    if ( parametersMatch )
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}