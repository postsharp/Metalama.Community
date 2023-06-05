// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Metalama.Framework.Aspects;

namespace Metalama.Community.Costura;

[CompileTime]
public class UnmanagedAssembly
{
    public UnmanagedAssembly( string name, UnmanagedAssemblyPlatform platform )
    {
        this.Name = name;
        this.Platform = platform;
    }

    public string Name { get; }

    public UnmanagedAssemblyPlatform Platform { get; }
}