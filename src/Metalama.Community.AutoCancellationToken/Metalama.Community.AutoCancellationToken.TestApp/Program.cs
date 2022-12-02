// This is an open-source Metalama example. See https://github.com/postsharp/Metalama.Samples for more.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Metalama.Community.AutoCancellationToken.TestApp;

internal class Program
{
    private static async Task Main()
    {
        var cts = new CancellationTokenSource( TimeSpan.FromSeconds( 6 ) );

        try
        {
            await MyClass.MakeRequests( cts.Token );
        }
        catch ( Exception ex )
        {
            Console.WriteLine( ex.Message );
        }
    }
}