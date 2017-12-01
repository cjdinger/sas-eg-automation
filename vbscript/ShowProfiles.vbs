' force declaration of variables in VB Script
Option Explicit
Dim Application
' Change if running a different version of EG
Dim egVersion 
egVersion = "SASEGObjectModel.Application.7.1"

' Create a new SAS Enterprise Guide automation session
Set Application = WScript.CreateObject(egVersion)
WScript.Echo Application.Name & ", Version: " & Application.Version

' Discover the available profiles that are defined for the current user
Dim i 
Dim oShell
Set oShell = CreateObject( "WScript.Shell" )
WScript.Echo "Metadata profiles available for " _
   & oShell.ExpandEnvironmentStrings("%UserName%")
WScript.Echo "----------------------------------------"
For i = 1 to Application.Profiles.Count-1
  WScript.Echo "Profile available: " _
    & Application.Profiles.Item(i).Name _
    & ", Host: " & Application.Profiles.Item(i).HostName _
	& ", Port: " & Application.Profiles.Item(i).Port
Next
Application.Quit