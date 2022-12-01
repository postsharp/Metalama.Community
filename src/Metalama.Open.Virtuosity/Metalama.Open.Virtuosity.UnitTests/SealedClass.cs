// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

#pragma warning disable CA1822 // Mark members as static

namespace Metalama.Open.Virtuosity.Tests.Struct
{
    // Transformed (sealed removed).
    [VirtualizeAttribute]
    internal sealed class SealedClass
    {
        // Transformed.
        public void M() { }
    }
}