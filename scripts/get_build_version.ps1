#!/usr/bin/env pwsh

Set-Variable -Name build_version_text -Value `
(Select-Xml -Path "./src/Directory.Build.props" -XPath "/Project/PropertyGroup/Version[contains(@Condition, `"`'`$(Version)`'==`'`'`")]//text()[1]").Node.Value -Option Constant;
Set-Variable -Name package_name -Value "Nuget.Versioning" -Option Constant;
Set-Variable -Name package_version -Value "6.3.0" -Option Constant;
Set-Variable -Name type_name -Value "NuGet.Versioning.NuGetVersion" -Option Constant;
Set-Variable -Name nuget_source -Value "https://api.nuget.org/v3/index.json" -Option Constant;

if($type_name -as [type]) {
  Write-Debug "{$type_name} already loaded";
  $ret = New-Object -TypeName $type_name -ArgumentList "$build_version_text";
  return $ret;
}

$nuget_versioning_source = (Get-Package $package_name).Source;
if ($error) {
  # Get nuget package and find the extraced dll in first occurence
  Write-Debug "try to download `"$package_name`" nuget package..."
  $error.Clear();
  Install-Package -Verbose -Name "$package_name" -RequiredVersion $package_version -SkipDependencies -ProviderName NuGet -Source $nuget_source -Scope CurrentUser -Force;
}

if($null -eq $nuget_versioning_source)
{
  Write-Debug "get package `"$package_name`" again";
  $nuget_versioning_source = (Get-Package $package_name).Source;
}
$nuget_dll_path = `
(Get-ChildItem -File -Filter "$package_name.dll" -Recurse (Split-Path $nuget_versioning_source) | Where-Object { $_.Directory -like "*netstandard2.0*"}).FullName;

Write-Debug "`"$package_name.dll`" real path: $nuget_dll_path"

Add-Type -Path $nuget_dll_path
$ret = New-Object -TypeName $type_name -ArgumentList "$build_version_text";
return $ret;
