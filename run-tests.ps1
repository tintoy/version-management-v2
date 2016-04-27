Param(
	[flag] $Verbose
)

If (!$Verbose) {
	$quietFlag = '--quiet'
}
Else {
	$quietFlag = ''
}

$dnx = Get-Command dnx

Function Invoke-DnxTests([string] $ProjectName) {
	& $dnx -p ".\test\$ProjectName" test $quietFlag
}

Invoke-DnxTests -ProjectName VersionManagement.Tests
Invoke-DnxTests -ProjectName VersionManagement.FunctionalTests
