// This is an open-source Metalama example. See https://github.com/postsharp/Metalama.Samples for more.

// TODO: Once Metalama.Community.Virtuosity package has been released, this should not be necessary.
using Metalama.Open.Virtuosity;

namespace Metalama.Community.Virtuosity.TestApp
{
    [Virtualize]
    public sealed class Test
    {
        public string Method()
        {
            return "Test.Method";
        }
    }
}