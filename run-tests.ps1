Param(
	[switch] $Verbose
)

If ($Verbose) {
    $reporterSwitch = '-appveyor'
}
Else {
    $reporterSwitch = '-quiet'
}

$dnx = Get-Command dnx

Function Invoke-DnxTests([string] $ProjectName) {
	& $dnx -p ".\test\$ProjectName" test $reporterSwitch
}

Invoke-DnxTests -ProjectName VersionManagement.Tests

# Need to work out how to set the application base path for views.
Write-Warning 'Functional tests disabled for now.'
# Invoke-DnxTests -ProjectName VersionManagement.FunctionalTests
