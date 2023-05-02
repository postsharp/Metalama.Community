// Released under the MIT license. See the LICENSE.md file in the root directory of this repository root for details.

namespace Metalama.Community.AutoCancellationToken.UnitTests;

[AutoCancellationToken]
internal class MyClass
{
    public static async Task MakeRequests( CancellationToken ct )
    {
        using var client = new HttpClient();

        // After transformation, the call to MakeRequest should include a CancellationToken.

        await MakeRequest( client );
        Console.WriteLine( "request 1 succeeded" );
        await MakeRequest( client );
        Console.WriteLine( "request 2 succeeded" );
    }

    // After transformation, MakeRequest should contain a CancellationToken parameter, and the call to client.GetAsync
    // should include this argument.
    private static async Task MakeRequest( HttpClient client )
    {
        await client.GetAsync( "https://httpbin.org/delay/5" );
    }
}