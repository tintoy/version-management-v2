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
Invoke-DnxTests -ProjectName VersionManagement.FunctionalTests
