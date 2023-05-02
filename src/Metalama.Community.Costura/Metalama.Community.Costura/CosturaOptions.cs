// Released under the MIT license. See the LICENSE.md file in the root directory of this repository root for details.

using Metalama.Framework.Project;
using System;
using System.Collections.Immutable;

namespace Metalama.Community.Costura;

public sealed class CosturaOptions : ProjectExtension
{
    private bool _createTemporaryAssemblies;
    private bool _disableCleanup;
    private ImmutableArray<string> _excludedAssemblies = ImmutableArray<string>.Empty;

    // Intentionally not initialized to empty because a default value means that all assemblies are to be included unless they are excluded.
    private ImmutableArray<string> _includedAssemblies;

    private ImmutableArray<string> _preloadOrder = ImmutableArray<string>.Empty;
    private ImmutableArray<UnmanagedAssembly> _unmanagedAssemblies = ImmutableArray<UnmanagedAssembly>.Empty;
    private bool _useCompression = true;
    private bool _includeDebugSymbols;
    private bool _includeSatelliteAssemblies = true;

    /// <summary>
    ///     Gets or sets a value indicating whether the <c>.pdb</c> files should also be embedded. The default value is
    ///     <c>true</c>.
    /// </summary>
    public bool IncludeDebugSymbols
    {
        get => this._includeDebugSymbols;

        set
        {
            this.CheckWritable();
            this._includeDebugSymbols = value;
        }
    }

    /// <summary>
    ///     Gets or sets a value indicating whether resources embedded into the main assembly should be compressed. The default
    ///     value is <c>true</c>.
    /// </summary>
    public bool CompressResources
    {
        get => this._useCompression;

        set
        {
            this.CheckWritable();
            this._useCompression = value;
        }
    }

    /// <summary>
    ///     Gets or sets a value indicating whether
    ///     embedded assemblies are placed in the output folder anyway, even
    ///     though they aren't necessary anymore.
    /// </summary>
    public bool IsCleanupDisabled
    {
        get => this._disableCleanup;

        [Obsolete( "This option does not work." )]
        set
        {
            this.CheckWritable();
            this._disableCleanup = value;
        }
    }

    /// <summary>
    ///     Gets or sets a value indicating whether embedded assemblies should be copied to disk before loading them into
    ///     memory. This is helpful for some scenarios that expected an assembly to be loaded from a physical file.
    ///     For example, if some code checks the assembly's assembly location. The default value is <c>false</c>.
    /// </summary>
    public bool CreateTemporaryAssemblies
    {
        get => this._createTemporaryAssemblies;

        set
        {
            this.CheckWritable();
            this._createTemporaryAssemblies = value;
        }
    }

    /// <summary>
    ///     Gets or sets a value indicating whether satellite assemblies (typically containing localized resources,
    ///     i.e. with a name like 'resources.dll') should be included, The default value is <c>true</c>.
    /// </summary>
    public bool IncludeSatelliteAssemblies
    {
        get => this._includeSatelliteAssemblies;
        set
        {
            this.CheckWritable();
            this._includeSatelliteAssemblies = value;
        }
    }

    /// <summary>
    ///     Gets or sets the lists of assembly names to embed. The assembly name should not include the <c>.exe</c> or
    ///     <c>.dll</c> extension.
    ///     The names can include wildcards at the end of the name for partial matching. For example <c>System.*</c> will
    ///     exclude all assemblies that start with System..
    ///     By default, all references with Copy Local set to <c>true</c> are embedded.
    /// </summary>
    public ImmutableArray<string> IncludedAssemblies
    {
        get => this._includedAssemblies;

        set
        {
            this.CheckWritable();
            this._includedAssemblies = value;
        }
    }

    /// <summary>
    ///     Gets or sets the list of assembly names to exclude from embedding. The assembly name should not include the
    ///     <c>.exe</c> or  <c>.dll</c> extension.
    ///     The names can include wildcards at the end of the name for partial assembly name matching. For example
    ///     <c>System.*</c> will exclude all assemblies that start with System..
    /// </summary>
    public ImmutableArray<string> ExcludedAssemblies
    {
        get => this._excludedAssemblies;

        set
        {
            this.CheckWritable();
            this._excludedAssemblies = value;
        }
    }

    /// <summary>
    ///     Gets or sets the list of mixed-mode assemblies. These assemblies must be loaded differently than the other.
    /// </summary>
    public ImmutableArray<UnmanagedAssembly> UnmanagedAssemblies
    {
        get => this._unmanagedAssemblies;

        set
        {
            this.CheckWritable();
            this._unmanagedAssemblies = value;
        }
    }

    /// <summary>
    ///     Gets or sets the list of native libraries can be loaded by this add-in automatically.
    ///     To include a native library include it in your project as an
    ///     Embedded Resource in a folder called Costura32 or Costura64
    ///     depending on the bitness of the library.
    ///     Optionally you can also specify the order that preloaded
    ///     libraries are loaded. When using temporary assemblies
    ///     from disk mixed mode assemblies are also preloaded.
    /// </summary>
    public ImmutableArray<string> PreloadOrder
    {
        get => this._preloadOrder;
        set
        {
            this.CheckWritable();
            this._preloadOrder = value;
        }
    }

    private void CheckWritable()
    {
        if ( this.IsReadOnly )
        {
            throw new InvalidOperationException( "Cannot modify this CosturaOptions because it is read-only." );
        }
    }
}