// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Metalama.Compiler;
using Metalama.Framework.Code;
using Metalama.Framework.Diagnostics;
using Metalama.Framework.Engine.CodeModel;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Metalama.Community.ConfigureAwaitWarning.Service;

[MetalamaPlugIn]
public sealed class ConfigureAwaitWarningService : IConfigureAwaitWarningService
{
    public IEnumerable<IDiagnosticLocation> GetWarningLocations( IDeclaration declaration )
    {
        Debugger.Break();

        var compilation = declaration.Compilation.GetPartialCompilation().Compilation;

        var forbiddenTypes = new[] { typeof(Task), typeof(Task<>), typeof(ValueTask), typeof(ValueTask<>) }
            .Select( type => compilation.GetTypeByMetadataName( type.FullName )! )
            .ToImmutableArray();

        var symbol = declaration.GetSymbol() ?? throw new InvalidOperationException( $"Can't produce warning locations for {declaration}, because it doesn't have a symbol." );

        foreach ( var syntaxReference in symbol.DeclaringSyntaxReferences )
        {
            var semanticModel = compilation.GetSemanticModel( syntaxReference.SyntaxTree );
            var walker = new Walker( semanticModel, forbiddenTypes );

            walker.Visit( syntaxReference.GetSyntax() );

            foreach ( var location in walker.WarningLocations )
            {
                yield return location;
            }
        }
    }

    private class Walker : CSharpSyntaxWalker
    {
        private readonly SemanticModel _semanticModel;
        private readonly ImmutableArray<INamedTypeSymbol> _forbiddenTypes;

        public Walker( SemanticModel semanticModel, ImmutableArray<INamedTypeSymbol> forbiddenTypes )
        {
            this._semanticModel = semanticModel;
            this._forbiddenTypes = forbiddenTypes;
        }

        public List<IDiagnosticLocation> WarningLocations { get; } = new();

        public override void VisitAwaitExpression( AwaitExpressionSyntax node )
        {
            base.VisitAwaitExpression( node );

            var expressionType = this._semanticModel.GetTypeInfo( node.Expression ).Type;

            if ( this._forbiddenTypes.Any( forbiddenType => SymbolEqualityComparer.Default.Equals( forbiddenType, expressionType?.OriginalDefinition ) ) )
            {
                this.WarningLocations.Add( node.GetDiagnosticLocation() );
            }
        }
    }
}