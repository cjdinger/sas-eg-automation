' force declaration of variables in VB Script
Option Explicit
Dim Application
' Create a new SAS Enterprise Guide automation session
' Change if running a different version of EG
Dim egVersion 
egVersion = "SASEGObjectModel.Application.7.1"

Set Application = WScript.CreateObject(egVersion)
WScript.Echo Application.Name & ", Version: " & Application.Version

Application.Quit