// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using PostSharp.Engineering.BuildTools;
using PostSharp.Engineering.BuildTools.Build.Model;
using PostSharp.Engineering.BuildTools.Build.Solutions;
using PostSharp.Engineering.BuildTools.Dependencies.Model;
using Spectre.Console.Cli;

var product = new Product( Dependencies.MetalamaCommunity )
{
    Solutions = new Solution[]
    {
        new DotNetSolution( "Metalama.Community.sln" ) { CanFormatCode = true }
    },
    PublicArtifacts = Pattern.Create(
        "Metalama.Community.AutoCancellationToken.$(PackageVersion).nupkg",
        "Metalama.Community.AutoCancellationToken.Redist.$(PackageVersion).nupkg",
        "Metalama.Community.Costura.$(PackageVersion).nupkg",
        "Metalama.Community.Costura.Redist.$(PackageVersion).nupkg",
        "Metalama.Community.Virtuosity.$(PackageVersion).nupkg",
        "Metalama.Community.Virtuosity.Redist.$(PackageVersion).nupkg" ),
    Dependencies = new[] { Dependencies.PostSharpEngineering, Dependencies.Metalama },
    
    // MergePublisher disabled for 2023.1.
    // Configurations = Product.DefaultConfigurations
    //     .WithValue( BuildConfiguration.Public, new BuildConfigurationInfo(
    //         MSBuildName: "Release",
    //         RequiresSigning: true,
    //         PublicPublishers: Product.DefaultPublicPublishers.Add( new MergePublisher() ).ToArray() ) )
};

var commandApp = new CommandApp();

commandApp.AddProductCommands( product );

return commandApp.Run( args );