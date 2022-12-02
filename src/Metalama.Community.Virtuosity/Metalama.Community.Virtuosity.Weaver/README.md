## Metalama.Community.Virtuosity 
Allows you to make all target methods virtual without adding the virtual keyword to every one of them.

*This is a [Metalama](https://github.com/postsharp/Metalama) aspect. It modifies your code during compilation by using source weaving.*
 
[![CI badge](https://github.com/postsharp/Metalama.Community.Virtuosity/workflows/Full%20Pipeline/badge.svg)](https://github.com/postsharp/Metalama.Community.Virtuosity/actions?query=workflow%3A%22Full+Pipeline%22)

#### Example
Your code:
```csharp
class A 
{
  public void M1() { }
  protected void M2() { }
  private void M3() { }
}
```
What gets compiled:
```csharp
class A 
{
  public virtual void M1() { }
  protected virtual void M2() { }
  private void M3() { }
}
```

#### Installation
1. Install the NuGet package: `dotnet add package Metalama.Community.Virtuosity`.
2. Apply the aspect to the project by adding the following attribute to some C# file:

    ```c#
    [assembly: Metalama.Community.Virtuosity.VirtuosityAspect]
    ```

#### How to use

By annotating an assembly with `[VirtuosityAspect]`, you make all methods `virtual` (except those that can't be `virtual`, like `static` and `private` methods and methods on `struct`s).

For more details, see [Details](Details.md).