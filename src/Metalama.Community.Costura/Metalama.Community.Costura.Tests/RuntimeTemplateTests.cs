// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Metalama.Community.Costura.Tests;

/// <summary>
/// Verifies that the runtime templates injected into the user's compilation are valid C#.
/// </summary>
/// <remarks>
/// These templates ship as string constants, so a syntax error in one of them does not break our own build -
/// it breaks the build of whoever consumes the package, and only on the code path that selects that template.
/// Issue #101 was exactly that: four call sites in <c>TemplateWithUnmanagedHandler</c> had been corrupted into
/// <c>private init;.ReadExistingAssembly(...)</c>, and no test ever caused that template to be parsed.
/// </remarks>
public sealed class RuntimeTemplateTests
{
    /// <summary>
    /// Gets the name and content of every runtime template, read from the internal <c>Resources</c> class of the
    /// weaver assembly by reflection, so that this test covers exactly what the weaver injects.
    /// </summary>
    public static TheoryData<string> TemplateNames
    {
        get
        {
            var data = new TheoryData<string>();

            foreach ( var templateField in GetTemplateFields() )
            {
                data.Add( templateField.Name );
            }

            return data;
        }
    }

    [Theory]
    [MemberData( nameof(TemplateNames) )]
    public void TemplateIsValidCSharp( string templateName )
    {
        var template = GetTemplateFields().Single( f => f.Name == templateName );
        var source = (string) template.GetValue( null )!;

        Assert.False( string.IsNullOrWhiteSpace( source ), $"Template '{templateName}' is empty." );

        var syntaxTree = CSharpSyntaxTree.ParseText( source );

        var errors = syntaxTree.GetDiagnostics()
            .Where( d => d.Severity == DiagnosticSeverity.Error )
            .ToList();

        Assert.True(
            errors.Count == 0,
            $"Template '{templateName}' is not valid C#:\n" +
            string.Join(
                "\n",
                errors.Select( e => $"  {e.Location.GetLineSpan()}: {e.Id} {e.GetMessage( CultureInfo.InvariantCulture )}" ) ) );
    }

    [Fact]
    public void AllTemplatesAreCovered()
    {
        // Guards against the template set silently shrinking, which would make the theory above vacuously green.
        var names = GetTemplateFields().Select( f => f.Name ).ToList();

        Assert.Contains( "Common", names );
        Assert.Contains( "Template", names );
        Assert.Contains( "TemplateWithTempAssembly", names );
        Assert.Contains( "TemplateWithUnmanagedHandler", names );
        Assert.Contains( "ModuleInitializer", names );
    }

    private static IEnumerable<FieldInfo> GetTemplateFields()
    {
        var resourcesType = Assembly.Load( "Metalama.Community.Costura.Weaver" )
            .GetType( "Metalama.Community.Costura.Weaver.Resources" );

        Assert.NotNull( resourcesType );

        return resourcesType!
            .GetFields( BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static )
            .Where( f => f.IsLiteral && f.FieldType == typeof(string) )
            .OrderBy( f => f.Name );
    }
}
