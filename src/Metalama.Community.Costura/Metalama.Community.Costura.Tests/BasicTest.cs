// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Metalama.Community.Costura.Tests;

/// <summary>
/// End-to-end tests that run a woven executable with its dependencies deleted from disk, so that the process can
/// only start if Costura embedded them and the injected resolver loads them from resources.
/// </summary>
public sealed class BasicTest
{
#if DEBUG
    private const string _configuration = "Debug";
#else
    private const string _configuration = "Release";
#endif

    private readonly string _folder = Environment.CurrentDirectory;
    private readonly ITestOutputHelper _logger;

    public BasicTest( ITestOutputHelper logger )
    {
        this._logger = logger;
    }

    [Fact]
    public void TestTestAssemblyWithReferences()
    {
        const string filename =
            $@"..\..\..\..\Metalama.Community.Costura.TestApp\bin\{_configuration}\net48\Metalama.Community.Costura.TestApp.exe";

        // The app references Newtonsoft.Json and Soothsilver.Random. Deleting everything but the executable means a
        // missing or broken embed shows up as a failure to resolve those assemblies at start-up.
        var deleted = DeleteAllButExes( filename );
        Assert.NotEmpty( deleted );

        this.RunToCompletion( filename, TimeSpan.FromSeconds( 30 ) );
    }

    [Fact]
    public void TestWpf()
    {
        const string filename =
            $@"..\..\..\..\Metalama.Community.Costura.WpfApp\bin\{_configuration}\net48\Metalama.Community.Costura.WpfApp.exe";

        DeleteAllButExes( filename );

        this.RunToCompletion( filename, TimeSpan.FromSeconds( 60 ) );
    }

    /// <summary>
    /// Runs the given executable and asserts that it exits with code 0 within <paramref name="timeout" />.
    /// </summary>
    /// <remarks>
    /// The process is always disposed, and killed if it does not exit in time. Leaving it running would keep a lock
    /// on the executable in the build tree and make every subsequent build and test run fail.
    /// </remarks>
    private void RunToCompletion( string filename, TimeSpan timeout )
    {
        var path = Path.Combine( this._folder, filename );

        Assert.True( File.Exists( path ), $"The executable '{path}' does not exist. Build the solution first." );

        var startInfo = new ProcessStartInfo( path )
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        using var process = Process.Start( startInfo )!;

        var output = new StringBuilder();

        process.OutputDataReceived += ( _, e ) =>
        {
            if ( e.Data != null )
            {
                lock ( output )
                {
                    output.AppendLine( e.Data );
                }
            }
        };

        process.ErrorDataReceived += ( _, e ) =>
        {
            if ( e.Data != null )
            {
                lock ( output )
                {
                    output.AppendLine( e.Data );
                }
            }
        };

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        if ( !process.WaitForExit( (int) timeout.TotalMilliseconds ) )
        {
            try
            {
                process.Kill();
            }
            catch ( InvalidOperationException )
            {
                // The process exited between the check and the kill.
            }

            Assert.Fail( $"'{Path.GetFileName( path )}' did not exit within {timeout.TotalSeconds:F0} s.{FormatOutput( output )}" );
        }

        // Let the asynchronous readers drain before reading the buffer.
        process.WaitForExit();

        this._logger.WriteLine( FormatOutput( output ) );

        Assert.True(
            process.ExitCode == 0,
            $"'{Path.GetFileName( path )}' exited with code {process.ExitCode}.{FormatOutput( output )}" );
    }

    private static string FormatOutput( StringBuilder output )
    {
        lock ( output )
        {
            return output.Length == 0 ? " The process produced no output." : $"\nProcess output:\n{output}";
        }
    }

    /// <summary>
    /// Deletes every file next to the executable except executables themselves, and returns what was deleted.
    /// </summary>
    private static string[] DeleteAllButExes( string file )
    {
        var directory = Path.GetDirectoryName( file )!;
        var deleted = new System.Collections.Generic.List<string>();

        foreach ( var filename in Directory.EnumerateFiles( directory ).ToList() )
        {
            if ( !filename.EndsWith( ".exe", StringComparison.OrdinalIgnoreCase ) )
            {
                File.Delete( filename );
                deleted.Add( Path.GetFileName( filename ) );
            }
        }

        return deleted.ToArray();
    }
}
