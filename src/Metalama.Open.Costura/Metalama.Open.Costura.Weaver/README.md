## Metalama.Open.Costura

Embeds dependencies as resources so that you can have a standalone executable.

This source transformer only works under .NET Framework. In .NET Core, we recommend upgrading to .NET Core 3 or later
and using [the single file executable feature](https://docs.microsoft.com/en-us/dotnet/core/deploying/single-file)
instead.

*This is a [Metalama](https://github.com/postsharp/Metalama) aspect. It modifies your code during compilation by using
source weaving.*

[![CI badge](https://github.com/postsharp/Metalama.Open.Costura/workflows/Full%20Pipeline/badge.svg)](https://github.com/postsharp/Metalama.Open.Costura/actions?query=workflow%3A%22Full+Pipeline%22)

#### Example

Assume that `MyProject` has dependencies on the `Newtonsoft.Json` and `Soothsilver.Random` packages.
Normally, `MyProject.exe` would require the files `Newtonsoft.Json.dll` and `Soothsilver.Random.dll` to work.

If you use this aspect, instead those two DLLs will be embedded into `MyProject.exe` as resources and loaded from there.
The only file you need is `MyProject.exe`.

#### Installation

1. Install the NuGet package: `dotnet add package Metalama.Open.Costura`.
2. Add the following code somewhere in your code:

    ```cs
    using Metalama.Framework.Fabrics;
    using Metalama.Open.Costura;
    
    internal class Fabric : ProjectFabric
    {
        public override void AmendProject(IProjectAmender amender)
        {
            amender.UseCostura();
        }
    }
    ```

You can then distribute just the main output assembly file. It will be enough.

There are documented configuration options in the `Costura` attribute. Set them in your source code to change
them from their defaults.