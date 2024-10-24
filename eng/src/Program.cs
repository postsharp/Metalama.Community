﻿// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using PostSharp.Engineering.BuildTools;
using PostSharp.Engineering.BuildTools.Build.Model;
using PostSharp.Engineering.BuildTools.Build.Solutions;
using PostSharp.Engineering.BuildTools.Dependencies.Definitions;
using MetalamaDependencies = PostSharp.Engineering.BuildTools.Dependencies.Definitions.MetalamaDependencies.V2024_2;

var product = new Product( MetalamaDependencies.MetalamaCommunity )
{
    Solutions =
    [
        new DotNetSolution( "Metalama.Community.sln" ) { CanFormatCode = true }
    ],
    PublicArtifacts = Pattern.Create(
        "Metalama.Community.AutoCancellationToken.$(PackageVersion).nupkg",
        "Metalama.Community.Costura.$(PackageVersion).nupkg",
        "Metalama.Community.Costura.Redist.$(PackageVersion).nupkg",
        "Metalama.Community.Virtuosity.$(PackageVersion).nupkg" ),
    Dependencies = [DevelopmentDependencies.PostSharpEngineering, MetalamaDependencies.Metalama]
};

return new EngineeringApp( product ).Run( args );