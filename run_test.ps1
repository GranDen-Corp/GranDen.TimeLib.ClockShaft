#!/usr/bin/env pwsh
param (
  [switch] $CollectCoverage = $false
)
Get-ChildItem test/*Tests.csproj -Recurse | ForEach-Object { dotnet test $_.FullName /p:CollectCoverage=$CollectCoverage }
