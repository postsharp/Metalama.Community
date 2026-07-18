// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

#pragma warning disable VSTHRD200, CA1822

using System.Collections.Generic;

namespace Metalama.Community.AutoCancellationToken.UnitTests.AsyncIterator;

// An async iterator is transformed like any other async method, but the token is not wired up to the enumerator:
// the added parameter carries no [EnumeratorCancellation], so a token passed to WithCancellation is unconsumed.
// The compiler reports this as CS8425, which is captured in the baseline - so at least this limitation is visible.
[AutoCancellationToken]
internal class Iterator
{
    public async IAsyncEnumerable<int> StreamAsync()
    {
        await Task.Yield();

        yield return 1;
    }
}
