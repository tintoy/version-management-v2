Param(
    [switch] $Verbose
)

If ($Verbose) {
    $quietFlag = ''
}
Else {
    $quietFlag = '--quiet'
}

$dnu = Get-Command dnu

& $dnu build 'src\VersionManagement' $quietFlag
