// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Metalama.Framework.Aspects;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace Metalama.Community.AutoCancellationToken
{
    internal partial class AutoCancellationTokenWeaver
    {
        [CompileTime]
        private sealed class AnnotateNodesRewriter : RewriterBase
        {
            private readonly HashSet<SyntaxNode> _instancesNodes;

            public AnnotateNodesRewriter( IEnumerable<SyntaxNode> instancesNodes )
            {
                this._instancesNodes = new HashSet<SyntaxNode>( instancesNodes );
            }

            public static SyntaxAnnotation Annotation { get; } = new();

            protected override T VisitTypeDeclaration<T>( T node, Func<T, SyntaxNode?> baseVisit )
            {
                if ( !this._instancesNodes.Contains( node ) )
                {
                    return node;
                }

                return node.WithAdditionalAnnotations( Annotation );
            }
        }
    }
}