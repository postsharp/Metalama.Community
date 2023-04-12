## Metalama.Community.ConfigureAwaitWarning 
TODO

*This is a [Metalama](https://github.com/postsharp/Metalama) aspect. It modifies your code during compilation by using source weaving.*
 
<!-- [![CI badge](https://github.com/postsharp/Metalama.Community.Virtuosity/workflows/Full%20Pipeline/badge.svg)](https://github.com/postsharp/Metalama.Community.Virtuosity/actions?query=workflow%3A%22Full+Pipeline%22) -->

#### Example

#### Installation
1. Install the NuGet package: `dotnet add package Metalama.Community.ConfigureAwaitWarning`.
2. Apply the aspect to the relavant declarations by adding the `[ConfigureAwaitWarning]` attribute to them.

#### How to use

By annotating an assembly, a type or a member with `[ConfigureAwaitWarning]`, you get a warning on any `await`s that don't explicitly use `ConfigureAwait(false)` or `ConfigureAwait(true)`.