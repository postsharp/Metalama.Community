// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace Metalama.Community.AutoCancellationToken.Weaver
{
    public partial class AutoCancellationTokenWeaver
    {
        private sealed class AddCancellationTokenArgumentRewriter : RewriterBase
        {
            private readonly Compilation _compilation;
            private readonly SyntaxAnnotation _generatedCodeAnnotation;
            private string? _cancellationTokenParameterName;

            public AddCancellationTokenArgumentRewriter(
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

            public override SyntaxNode? VisitMethodDeclaration( MethodDeclarationSyntax node )
            {
                var semanticModel = this._compilation.GetSemanticModel( node.SyntaxTree );

                var methodSymbol = semanticModel.GetDeclaredSymbol( node );

                if ( methodSymbol is not { IsAsync: true } )
                {
                    return node;
                }

                var cancellationTokenParameter = methodSymbol.Parameters.Where( IsCancellationToken ).LastOrDefault();

                if ( cancellationTokenParameter == null )
                {
                    return node;
                }

                this._cancellationTokenParameterName = cancellationTokenParameter.Name;

                try
                {
                    return base.VisitMethodDeclaration( node );
                }
                finally
                {
                    this._cancellationTokenParameterName = null;
                }
            }

            public override SyntaxNode VisitAnonymousMethodExpression( AnonymousMethodExpressionSyntax node )
                => VisitFunction( node, false, base.VisitAnonymousMethodExpression );

            public override SyntaxNode VisitParenthesizedLambdaExpression( ParenthesizedLambdaExpressionSyntax node )
                => VisitFunction(
                    node,
                    node.Modifiers.Any( SyntaxKind.StaticKeyword ),
                    base.VisitParenthesizedLambdaExpression );

            public override SyntaxNode VisitSimpleLambdaExpression( SimpleLambdaExpressionSyntax node )
                => VisitFunction(
                    node,
                    node.Modifiers.Any( SyntaxKind.StaticKeyword ),
                    base.VisitSimpleLambdaExpression );

            public override SyntaxNode VisitLocalFunctionStatement( LocalFunctionStatementSyntax node )
                => VisitFunction(
                    node,
                    node.Modifiers.Any( SyntaxKind.StaticKeyword ),
                    base.VisitLocalFunctionStatement );

            private static T VisitFunction<T>( T node, bool isStatic, Func<T, SyntaxNode?> baseVisit )
                where T : SyntaxNode
            {
                if ( isStatic )
                {
                    return node;
                }

                return (T) baseVisit( node )!;
            }

            public override SyntaxNode VisitInvocationExpression( InvocationExpressionSyntax node )
            {
                var mustAddArgument = false;

                var semanticModel = this._compilation.GetSemanticModel( node.SyntaxTree );

                var invocationWithCt =
                    node.AddArgumentListArguments( SyntaxFactory.Argument( SyntaxFactory.DefaultExpression( CancellationTokenType ) ) );

                var newInvocationArgumentsCount = invocationWithCt.ArgumentList.Arguments.Count;

                if (

                    // the code compiles
                    semanticModel.GetSpeculativeSymbolInfo( node.SpanStart, invocationWithCt, default ).Symbol is
                        IMethodSymbol speculativeSymbol &&

                    // the added parameter corresponds to its own argument
                    speculativeSymbol.Parameters.Length >= newInvocationArgumentsCount &&

                    // that argument is CancellationToken
                    IsCancellationToken( speculativeSymbol.Parameters[newInvocationArgumentsCount - 1] ) )
                {
                    mustAddArgument = true;
                }

                node = (InvocationExpressionSyntax?) base.VisitInvocationExpression( node )!;

                if ( mustAddArgument )
                {
                    var arguments = node.ArgumentList.Arguments.GetWithSeparators().ToList();

                    if ( arguments.Count > 0 )
                    {
                        // Remove the trivia after the last argument.
                        arguments[arguments.Count - 1] =
                            arguments[arguments.Count - 1].AsNode()!.WithoutTrailingTrivia();

                        arguments.Add(
                            SyntaxFactory.Token( SyntaxKind.CommaToken )
                                .WithTrailingTrivia( SyntaxFactory.ElasticSpace )
                                .WithAdditionalAnnotations( this._generatedCodeAnnotation ) );
                    }

                    arguments.Add(
                        SyntaxFactory.Argument( SyntaxFactory.IdentifierName( this._cancellationTokenParameterName! ) )
                            .WithAdditionalAnnotations( this._generatedCodeAnnotation )
                            .WithTrailingTrivia( SyntaxFactory.ElasticSpace ) );

                    node = node.WithArgumentList(
                        SyntaxFactory.ArgumentList( SyntaxFactory.SeparatedList<ArgumentSyntax>( new SyntaxNodeOrTokenList( arguments ) ) ) );
                }

                return node;
            }
        }
    }
}