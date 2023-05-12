# Metalama.Community.AutoCancellationToken 

Automatically propagates `CancellationToken` parameter to `async` methods and method calls within them.

*This is a [Metalama](https://github.com/postsharp/Metalama) aspect. It modifies your code during compilation by using source weaving.*


## Example

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

## Installation
Install the NuGet package: `dotnet add package Metalama.Community.AutoCancellationToken`.

## How to use

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

## Limitations

The principal objective of this project is educational. It  project has several limitations that  severely limit its interest for production use:

1. It does not take into account `virtual` methods.

    Consider this:
    
    ```cs
     class A {
      abstract Task M();
    }

     class B : A {
      override Task M() => File.ReadAsync("f.txt");
    }
    ```

   To add a `CancellationToken` to `B.M`, it must be done in `A.M`.

2. A proper implementation would need to implement a fixed point algorithm (commonly used in iterative static analysis).
  
    Consider for instance:
    
    ```cs
    Task A() => B();
    Task B() => C();
    Task C() => File.ReadAsync("f.txt");
    ```
    
    The algorithm should add a cancellation token to `C`, then transitively to `B`, then to `A`. This cannot be done with a single-pass algorithm (there needs to be an iterative analysis algorithm followed by a single-pass rewriting algorithm).


