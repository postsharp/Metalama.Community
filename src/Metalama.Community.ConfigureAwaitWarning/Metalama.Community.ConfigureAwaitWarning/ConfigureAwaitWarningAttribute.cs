// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Diagnostics;
using Metalama.Framework.Eligibility;
using Metalama.Framework.Project;

namespace Metalama.Community.ConfigureAwaitWarning;

public class ConfigureAwaitWarningAttribute : Aspect, IAspect<IDeclaration>
{
    private static readonly DiagnosticDefinition _warning = new( "MY001", Severity.Warning, "Await expression cannot be used without ConfigureAwait." );

    public void BuildAspect( IAspectBuilder<IDeclaration> builder )
    {
        var service = builder.Project.ServiceProvider.GetRequiredService<IConfigureAwaitWarningService>();

        foreach ( var warningLocation in service.GetWarningLocations( builder.Target ) )
        {
            builder.Diagnostics.Report( _warning, warningLocation );
        }
    }

    public void BuildEligibility( IEligibilityBuilder<IDeclaration> builder )
    {
    }
}