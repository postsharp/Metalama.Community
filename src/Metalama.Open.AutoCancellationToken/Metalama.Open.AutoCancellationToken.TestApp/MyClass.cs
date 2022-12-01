using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Metalama.Open.AutoCancellationToken.TestApp;

[AutoCancellationToken]
internal class MyClass
{
    public static async Task MakeRequests( CancellationToken ct )
    {
        using var client = new HttpClient();
        await MakeRequest( client );
        Console.WriteLine( "request 1 succeeded" );
        await MakeRequest( client );
        Console.WriteLine( "request 2 succeeded" );
    }

    private static async Task MakeRequest( HttpClient client ) => await client.GetAsync( "https://httpbin.org/delay/5" );
}