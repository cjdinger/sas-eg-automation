' ------------------------------------------------
' ExtractCode.vbs
' Uses SAS Enterprise Guide automation to read an EGP file
' and export all SAS programs to subfolders
' There is a new subfolder created for each process flow
' within the project
'
' This script uses the Code.SaveAs method, which will include
' other "wrapper" code around each program, including macro
' variables and definitions that are used by SAS Enterprise Guide
'
' USAGE:
'   cscript.exe ExtractCode.vbs <path-to-EGP-file>
' EXAMPLE:
'   cscript.exe ExtractCode.vbs c:\projects\DonorsChoose.egp
'
' NOTE: use proper version of CSCRIPT.exe for your SAS Enterprise Guide
' version.  For 32-bit EG on 64-bit Windows, use 
'      c:\Windows\SysWOW64\cscript.exe 
' ------------------------------------------------
' force declaration of variables in VB Script
Option Explicit
Dim Application
Dim Project

' Change if running a different version of EG
Dim egVersion 
egVersion = "SASEGObjectModel.Application.7.1"

' enumeration of project item types
Const egLog = 0  
Const egCode = 1  
Const egData = 2  
Const egQuery = 3  
Const egContainer = 4  
Const egDocBuilder = 5  
Const egNote = 6  
Const egResult = 7  
Const egTask = 8  
Const egTaskCode = 9  
Const egProjectParameter = 10  
Const egOutputData = 11  
Const egStoredProcess = 12  
Const egStoredProcessParameter = 13  
Const egPublishAction = 14  
Const egCube = 15  
Const egReport = 18  
Const egReportSnapshot = 19  
Const egOrderedList = 20  
Const egSchedule = 21  
Const egLink = 22  
Const egFile = 23  
Const egIntrNetApp = 24  
Const egInformationMap = 25  
 
If WScript.Arguments.Count = 0 Then
  WScript.Echo "ERROR: Expecting the full path name of a project file"
  WScript.Quit -1
End If

' Create a new SAS Enterprise Guide automation session
On Error Resume Next
Set Application = WScript.CreateObject(egVersion)
WScript.Echo "Opening project: " & WScript.Arguments.Item(0)

' Open the EGP file with the Application
Set Project = Application.Open(WScript.Arguments.Item(0),"")
If Err.Number <> 0 Then
  WScript.Echo "ERROR: Unable to open " _
    & WScript.Arguments.Item(0) & " as a project file"
  WScript.Quit -1
End If
Dim programsFolder
programsFolder = getWorkingDirectory() _
   & "\" & Project.Name & "_programs"
Wscript.Echo "Target folder for SAS programs is "  & programsFolder
createFolder(programsFolder)
Dim item 
Dim flow

' Navigate the process flows in the Project object
For Each flow In Project.ContainerCollection
  ' ProcessFlow is ContainerType of 0
  If flow.ContainerType = 0 Then
    Dim progNumber
	progNumber = 1
    WScript.Echo "Process Flow: " & flow.Name
	Dim flowFolder
    flowFolder = programsFolder & "\" & flow.Name	
	createFolder(flowFolder)
	' Navigate the items in each process flow
	For Each item in flow.Items
	  Select Case item.Type
	  Case egFile 
	    WScript.Echo "  " & item.Name & ", File Name: " & item.FileName
	  Case egCode 
	    WScript.Echo "  " & item.Name & ", SAS Program"  
		WScript.Echo "    saving program content to: " _
		   & flowFolder & "\" & progNumber & "_" & item.Name & ".sas"
		' SaveAs method will include Wrapper code, and will write content
		' to disk for you.
		' If not wanted, you can get just code with the Text property
		' and then use VBScript methods to save content to a file
		item.SaveAs flowFolder & "\" & progNumber & "_" & item.Name & ".sas"
		progNumber = progNumber + 1
		If Err.Number <> 0 Then
		  WScript.Echo "     ERROR: There was an error while saving " _
		    & flowFolder & "\" & item.Name & ".sas"
		  Err.Clear
		End If
	  Case egData
	    WScript.Echo "  " & item.Name & ", Data: " & item.FileName
	  End Select
	Next
  End If
Next

' Close the project
Project.Close
' Quit/close the Application object
Application.Quit

' --- Helper functions ----------------

' function to get the current working directory
Function getWorkingDirectory()
	Dim objShell
	Set objShell = CreateObject("Wscript.Shell")
	getWorkingDirectory = objShell.CurrentDirectory
End Function

' function to create a new subfolder, if it doesn't yet exist
Function createFolder(folderName)
  Dim objFSO
  Set objFSO = CreateObject("Scripting.FileSystemObject")
  If objFSO.FolderExists(folderName) = False Then
	objFSO.CreateFolder folderName
  End If
End Function