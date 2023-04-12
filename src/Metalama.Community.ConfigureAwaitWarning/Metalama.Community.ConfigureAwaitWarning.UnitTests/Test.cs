// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

#if TEST_OPTIONS
// @ReportOutputWarnings
#endif

using System.Threading.Tasks;

#pragma warning disable IDE0051 // Remove unused private members

namespace Metalama.Community.ConfigureAwaitWarning.UnitTests
{
    [ConfigureAwaitWarning]
    internal class Test
    {
        private static async Task M()
        {
            // Warning.
            await Task.Delay( 0 );

            // No warning.
            await Task.Delay( 0 ).ConfigureAwait( false );

            // Also no warning.
            await Task.Delay( 0 ).ConfigureAwait( continueOnCapturedContext: true );

            // YieldAwaitable cannot be configured, so no warning.
            await Task.Yield();
        }
    }
}