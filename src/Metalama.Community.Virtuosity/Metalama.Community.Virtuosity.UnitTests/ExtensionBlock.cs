// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

#pragma warning disable VSTHRD200, VSTHRD100

using System.Threading.Tasks;

#pragma warning disable CA1822 // Mark members as static

namespace Metalama.Community.Virtuosity.TestApp
{
    // The weaver must not crash on C# 14 extension blocks. Members declared inside an
    // extension block cannot be virtual, so they are not transformed (see issue #94).
    [Virtualize]
    internal static class ValueTaskExtensions
    {
        extension<T>(ValueTask<T> valueTask)
        {
            // Not transformed (extension members cannot be virtual).
            public ValueTask ToValueTask()
            {
                if (valueTask.IsCompletedSuccessfully)
                {
                    valueTask.GetAwaiter().GetResult();

                    return default;
                }

                return new ValueTask(valueTask.AsTask());
            }
        }
    }
}
