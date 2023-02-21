(& dotnet nuget locals http-cache -c) | Out-Null
& dotnet run --project "$PSScriptRoot\eng\src\BuildMetalamaCommunity.csproj" -- $args
exit $LASTEXITCODE

