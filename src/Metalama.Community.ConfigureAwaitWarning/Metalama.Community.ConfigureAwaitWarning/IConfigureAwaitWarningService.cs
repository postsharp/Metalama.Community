// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Diagnostics;
using Metalama.Framework.Services;

namespace Metalama.Community.ConfigureAwaitWarning;

public interface IConfigureAwaitWarningService : IProjectService
{
    IEnumerable<IDiagnosticLocation> GetWarningLocations( IDeclaration declaration );
}