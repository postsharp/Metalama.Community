## Metalama.Community.AutoCancellationToken 
Automatically propagates `CancellationToken` parameter to `async` methods and method calls within them.

*This is a [Metalama](https://github.com/postsharp/Metalama) aspect. It modifies your code during compilation by using source weaving.*

You can also [try this aspect on try.metalama.net](https://try.metalama.net/#autocancellationtoken).

[![CI badge](https://github.com/postsharp/Metalama.Community.AutoCancellationToken/workflows/Full%20Pipeline/badge.svg)](https://github.com/postsharp/Metalama.Community.AutoCancellationToken/actions?query=workflow%3A%22Full+Pipeline%22)

#### Example

Your code:

```csharp
[AutoCancellationToken]
class C
{
    async Task MakeRequests(CancellationToken ct)
    {
        using var client = new HttpClient();
        await MakeRequest(client);
    }

    private static async Task MakeRequest(HttpClient client) => await client.GetAsync("https://example.org");
}
```

What gets compiled:

```csharp
class C
{
    async Task MakeRequests(CancellationToken ct)
    {
        using var client = new HttpClient();
        await MakeRequest(client, ct);
    }

    private static async Task MakeRequest(HttpClient client, CancellationToken cancellationToken = default) => await client.GetAsync("https://example.org", cancellationToken);
}
```

Notice that `CancellationToken` parameter was added to the declaration of `MakeRequest` and that `CancellationToken`
argument was added to the calls of `MakeRequest` and `HttpClient.GetAsync`.

#### Installation
Install the NuGet package: `dotnet add package Metalama.Community.AutoCancellationToken`.

#### How to use

Add `[AutoCancellationToken]` to the types where you want it to apply.

By annotating a type with `[AutoCancellationToken]`, you add cancellation to all its `async` methods. Specifically:

* A `CancellationToken` parameter is added to all `async` methods that don't have it.
* A `CancelltionToken` argument is added to calls within `async` methods where:
    * `CancellationToken` can be added as a last argument and the added argument corresponds to a `CancellationToken`
      parameter (e.g. it's not a `params object[]` parameter or a generic parameter). The added argument can result in
      calling a different overload of the method, or specifying a value for an optional parameter.
    * The call is not in a `static` local function.
    * The containing method doesn't have two or more `CancellationToken` parameters, since it wouldn't be clear which
      one to use.
