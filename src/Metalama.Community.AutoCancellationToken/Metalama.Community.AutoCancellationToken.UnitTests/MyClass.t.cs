namespace Metalama.Community.AutoCancellationToken.UnitTests;

[AutoCancellationToken]
internal class MyClass
{
    public static async Task MakeRequests( CancellationToken ct )
    {
        using var client = new HttpClient();

        // After transformation, the call to MakeRequest should include a CancellationToken.

        await MakeRequest(client, ct );
        Console.WriteLine( "request 1 succeeded" );
        await MakeRequest(client, ct );
        Console.WriteLine( "request 2 succeeded" );
    }

    // After transformation, MakeRequest should contain a CancellationToken parameter, and the call to client.GetAsync
    // should include this argument.
    private static async Task MakeRequest(HttpClient client, System.Threading.CancellationToken cancellationToken = default )    {
        await client.GetAsync("https://httpbin.org/delay/5", cancellationToken );
    }
}