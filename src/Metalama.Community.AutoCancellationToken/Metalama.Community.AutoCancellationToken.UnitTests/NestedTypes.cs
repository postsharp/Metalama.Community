// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

#pragma warning disable VSTHRD200, CA1822

namespace Metalama.Community.AutoCancellationToken.UnitTests.NestedTypes;

// This test pins CURRENT behaviour, which is known to be wrong for nested types: the aspect is silently ignored on
// a nested type even when the nested type carries the attribute itself. See the linked issue. The baseline below
// will change when that is fixed, which is the point of pinning it here.
[AutoCancellationToken]
internal class Outer
{
    // Transformed.
    public async Task OuterAsync() => await Helper();

    // Not transformed, as expected: the attribute applies to the members of the type it is applied to.
    internal class NestedNotAnnotated
    {
        public async Task NestedAsync() => await Helper();
    }

    // Not transformed, although it should be: the aspect is silently ignored on nested types.
    [AutoCancellationToken]
    internal class NestedAnnotated
    {
        public async Task NestedAsync() => await Helper();
    }

    private static async Task Helper() => await Task.Yield();

    private static async Task Helper( CancellationToken cancellationToken ) => await Task.Yield();
}
