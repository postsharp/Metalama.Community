// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Metalama.Compiler;
using Metalama.Framework.Engine.AspectWeavers;
using Metalama.Framework.Engine.CodeModel;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using System.Threading.Tasks;

namespace Metalama.Community.AutoCancellationToken.Weaver
{
    [MetalamaPlugIn]
    public partial class AutoCancellationTokenWeaver : IAspectWeaver
    {
        public async Task TransformAsync( AspectWeaverContext context )
        {
            var compilation = context.Compilation;
            var instancesNodes = context.AspectInstances.SelectMany( a => a.Key.DeclaringSyntaxReferences ).Select( x => x.GetSyntax() );

            await RunRewriterAsync( new AnnotateNodesRewriter( instancesNodes ) );
            await RunRewriterAsync( new AddCancellationTokenParameterRewriter( compilation.Compilation, context.GeneratedCodeAnnotation ) );
            await RunRewriterAsync( new AddCancellationTokenArgumentRewriter( compilation.Compilation, context.GeneratedCodeAnnotation ) );

            context.Compilation = compilation;

            async Task RunRewriterAsync( CSharpSyntaxRewriter rewriter )
            {
                compilation = await compilation.RewriteSyntaxTreesAsync( rewriter, context.ServiceProvider );
            }
        }
    } 
}