// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Metalama.Framework.Aspects;
using Metalama.Framework.Fabrics;

namespace Metalama.Community.Costura;

[CompileTime]
public static class CosturaProjectExtensions
{
    public static void UseCostura(
        this IProjectAmender projectAmender,
        CosturaOptions? options = null )
    {
        if ( options != null )
        {
            projectAmender.Outbound.SetOptions( _ => options );
        }

        projectAmender.Outbound.AddAspect<CosturaAspect>();
    }
}