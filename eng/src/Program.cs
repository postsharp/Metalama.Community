// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using PostSharp.Engineering.BuildTools;
using PostSharp.Engineering.BuildTools.Build;
using PostSharp.Engineering.BuildTools.Build.Model;
using PostSharp.Engineering.BuildTools.Build.Solutions;
using PostSharp.Engineering.BuildTools.Dependencies.Definitions;
using PostSharp.Engineering.BuildTools.Docker;
using System;
using MetalamaDependencies = PostSharp.Engineering.BuildTools.Dependencies.Definitions.MetalamaDependencies.V2027_0;

// The only .NET SDK of the build agent, and the one pinned in global.json. The version comes from the product
// family, so that it matches the feature band that the Visual Studio version of the family installs.
var dotNetSdkVersion = MetalamaDependencies.Family.PreferredVersions.DotNetSdk.V_10_0;

var product = new Product( MetalamaDependencies.MetalamaCommunity )
{
    OverriddenBuildAgentRequirements = new ContainerRequirements( ContainerHostKind.Windows )
    {
        Components =
        [
            new DotNetComponent( dotNetSdkVersion, DotNetComponentKind.Sdk ),
        ]
    },
    GenerateNuGetConfig = true,
    DotNetSdkVersion = new DotNetSdkVersion( dotNetSdkVersion ),
    MSBuildVersion = new Version( 17, 14 ),
    
    Solutions =
    [
        new DotNetSolution( "Metalama.Community.sln" ) { CanFormatCode = true },

        // A standalone test: it consumes the produced NuGet package instead of a project reference, so it covers
        // the packaging itself - analyzer wiring, dependencies, .props/.targets - which the main solution cannot.
        new DotNetSolution(
            @"src\Metalama.Community.Virtuosity\Metalama.Community.Virtuosity.TestApp\Metalama.Community.Virtuosity.TestApp.sln" )
        {
            IsTestOnly = true
        }
    ],
    PublicArtifacts = Pattern.Create(
        "Metalama.Community.AutoCancellationToken.$(PackageVersion).nupkg",
        "Metalama.Community.Costura.$(PackageVersion).nupkg",
        "Metalama.Community.Costura.Redist.$(PackageVersion).nupkg",
        "Metalama.Community.Virtuosity.$(PackageVersion).nupkg" ),
};

return new EngineeringApp( product ).Run( args );