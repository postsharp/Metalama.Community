// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Metalama.Framework.Aspects;
using Metalama.Framework.Fabrics;
using System;

namespace Metalama.Community.Costura;

[CompileTime]
public static class CosturaProjectExtensions
{
    public static void UseCostura(
        this IProjectAmender projectAmender,
        Action<CosturaOptions>? configure = null )
    {
        var options = projectAmender.Project.Extension<CosturaOptions>();

        configure?.Invoke( options );

        projectAmender.Outbound.AddAspect<CosturaAspect>();
    }
}