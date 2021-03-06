# Usage: 
#  Run ListProfiles.ps1 from a PowerShell command prompt
#  or run
#    powershell -command "ListProfiles.ps1"
#
# change version if running a different version of EG
$eguideApp = New-Object -comObject SASEGObjectModel.Application.7.1
Write-Host $eguideApp.Name  $eguideApp.Version
Write-Host "Profiles defined for:" $env:UserName
foreach ($profile in $eguideApp.Profiles())
{
  Write-Host "Profile: Name=" $profile.Name 
  Write-Host "  Host=" $profile.HostName "Port=" $profile.Port
}