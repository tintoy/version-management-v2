$dotnet = Get-Command dotnet
& $dotnet build 'src\VersionManagement' $quietFlag
