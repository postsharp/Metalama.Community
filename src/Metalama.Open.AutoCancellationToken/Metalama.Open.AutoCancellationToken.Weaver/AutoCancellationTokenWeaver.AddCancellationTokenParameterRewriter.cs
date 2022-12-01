// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace Metalama.Open.AutoCancellationToken.Weaver
{
    public partial class AutoCancellationTokenWeaver
    {
        private sealed class AddCancellationTokenParameterRewriter : RewriterBase
        {
            private readonly Compilation _compilation;
            private readonly SyntaxAnnotation _generatedCodeAnnotation;

            public AddCancellationTokenParameterRewriter( Compilation compilation, SyntaxAnnotation generatedCodeAnnotation )
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

                if ( methodSymbol == null || !methodSymbol.IsAsync || methodSymbol.Parameters.Any( IsCancellationToken ) )
                {
                    return node;
                }

                var parameters = node.ParameterList.Parameters.GetWithSeparators().ToList();

                if ( parameters.Count > 0 )
                {
                    // Remove the trivia after the last argument.
                    parameters[parameters.Count - 1] = parameters[parameters.Count - 1].AsNode()!.WithoutTrailingTrivia();

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
                            SyntaxFactory.Identifier( "cancellationToken" ).WithTrailingTrivia( SyntaxFactory.ElasticSpace ),
                            SyntaxFactory.EqualsValueClause(
                                    SyntaxFactory.Token( SyntaxKind.EqualsToken ).WithTrailingTrivia( SyntaxFactory.ElasticSpace ),
                                    SyntaxFactory.LiteralExpression( SyntaxKind.DefaultLiteralExpression ) )
                                .WithTrailingTrivia( SyntaxFactory.ElasticSpace ) )
                        .WithTrailingTrivia( SyntaxFactory.ElasticSpace )
                        .WithAdditionalAnnotations( this._generatedCodeAnnotation ) );

                node = node.WithParameterList(
                    SyntaxFactory.ParameterList( SyntaxFactory.SeparatedList<ParameterSyntax>( new SyntaxNodeOrTokenList( parameters ) ) ) );

                return node;
            }
        }
    }
}