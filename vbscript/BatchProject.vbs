Option Explicit ' Forces us to declare all variables

' Change if running a different version of EG
Dim egVersion 
egVersion = "SASEGObjectModel.Application.7.1"

Dim Application ' Application
Dim Project     ' Project object
Dim sasProgram  ' Code object (SAS program)
Dim n           ' counter

Set Application = CreateObject(egVersion)
' Set to your metadata profile name, or "Null Provider" for just Local server
Application.SetActiveProfile("My Server")
' Create a new Project
Set Project = Application.New 
' add a new code object to the Project
Set sasProgram = Project.CodeCollection.Add
 
' set the results types, overriding Application defaults
sasProgram.UseApplicationOptions = False
sasProgram.GenListing = True
sasProgram.GenSasReport = False
sasProgram.GenHtml = False
 
' Set the server (by Name) and text for the code
sasProgram.Server = "SASApp"
' Create the SAS program to run
sasProgram.Text = "data work.cars; set sashelp.cars; if ranuni(0)<0.85; run;"
sasProgram.Text =  sasProgram.Text & " proc means; run;"
 
' Run the code
sasProgram.Run
' Save the log file to LOCAL disk
sasProgram.Log.SaveAs _
  getCurrentDirectory & "\" & WScript.ScriptName & ".log"
 
 ' Save all output data as local Excel files
For n=0 to (sasProgram.OutputDatasets.Count -1)
  Dim dataName
  dataName = sasProgram.OutputDatasets.Item(n).Name
  sasProgram.OutputDatasets.Item(n).SaveAs _
    getCurrentDirectory & "\" & dataName & ".xls"
Next

' Filter through the results and save just the LISTING type
For n=0 to (sasProgram.Results.Count -1)
	' Listing type is 7
	If sasProgram.Results.Item(n).Type = 7 Then
		' Save the listing file to LOCAL disk
		sasProgram.Results.Item(n).SaveAs _
		   getCurrentDirectory & "\" & WScript.ScriptName & ".lst"
	End If
Next

Application.Quit

' function to fetch the current directory
Function getCurrentDirectory
	Dim oFSO
	Set oFSO = CreateObject("Scripting.FileSystemObject")
	getCurrentDirectory = _
	  oFSO.GetParentFolderName(WScript.ScriptFullName)
End Function