' ------------------------------------------------
' ExtractCode.vbs
' Derived from original code from Chris Hemedinger (cjdinger)
' https://github.com/cjdinger/sas-eg-automation/blob/master/vbscript/ExtractCode.vbs
'
' This program extracts code from EGP file quite differently from original code:
' Uses SAS Enterprise Guide automation to read an EGP file
' and export all SAS programs to SAS files
' There is a new SAS file created for each process flow
' within the project
' The internal nodes are output to the SAS file, concatenated 
' and sorted in alpha-numeric order by node name
'
' This script uses the Scripting.FileSystemObject to save the code, 
' _which_WILL_NOT_ include
' other "wrapper" code around each program, including macro
' variables and definitions that are used by SAS Enterprise Guide
' An exception to this is that the _CLIENTTASKLABEL is included to recognize 
' the name of the node (perhaps slightly altered) and is given without quotes.
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
' WScript.Echo "Opening project: " & WScript.Arguments.Item(0)

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
DIm Unsorted
Dim Items_Sorted
Dim FlowFilename
Dim TxtOutput
Dim task
Dim i
Dim nodeNumber


Set Unsorted = CreateObject("System.Collections.ArrayList")

' Navigate the process flows in the Project object
For Each flow In Project.ContainerCollection
  ' ProcessFlow is ContainerType of 0
  If flow.ContainerType = 0 Then
    'TxtOutput will be saved in FlowFilename
      FlowFilename = programsFolder & "\" & flow.Name & ".sas"
      WScript.Echo "Process Flow: " & FlowFilename
      TxtOutput = ""
      nodeNumber = 0
	' Navigate the items in each process flow
	Unsorted.Clear()
	For Each item in flow.Items
	  Unsorted.Add item
	Next
	Set Items_Sorted = SortArrayList_ByName(Unsorted)
	for i=0 to Items_Sorted.Count -1
	  Set item=Items_Sorted(i)
	  Select Case item.Type

	    Case egCode 
	      WScript.Echo "  " & item.Name
		  TxtOutput = TxtOutput & _
		    "%LET _CLIENTTASKLABEL=" & strClean(item.Name) & ";" & vbCrLf & item.Text & vbCrLf
              nodeNumber=nodeNumber+1
	      
	    Case egTask, egQuery
	      WScript.Echo "  " & item.Name & ", Task/Query"
		  If (Not item.TaskCode is Nothing) Then
		    TxtOutput = TxtOutput & _
		      "%LET _CLIENTTASKLABEL=" & strClean(item.Name) & ";" & vbCrLf & item.TaskCode.Text & vbCrLf
		  End If
              nodeNumber=nodeNumber+1
  
	    End Select
	Next
     if nodeNumber>0 then _
       saveTextToFile FlowFilename, TxtOutput
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

' Save a block of text (code or log) to text file
Function saveTextToFile(fileName, text)
  Dim objFS
  Dim objOutFile
  Set objFS = CreateObject("Scripting.FileSystemObject")
  Set objOutFile = objFS.CreateTextFile(fileName,True)
  objOutFile.Write(text)
  objOutFile.Close
  IF_Saving_Error_THEN_Report Err.Number, fileName
End Function

Function IF_Saving_Error_THEN_Report  ( byRef Err_Number, fileName)
  If Err.Number <> 0 Then
    WScript.Echo "     ERROR: There was an error while saving " & fileName
    Err.Clear
  End If
End Function

'Sort an array of object pointers that have the Name Property
Function SortArrayList_ByName(ByVal array_)
    Dim i, j, temp
   ' WScript.Echo array_(1).Name
    For i = (array_.Count - 1) to 0 Step -1
	    'WScript.Echo "i-"&i
        For j= 0 to i-1
		'WScript.Echo "j-"&j
            If array_(j).Name > array_(j+1).Name Then
                set temp = array_(j+1)
                set array_(j+1) = array_(j)
                set array_(j) = temp
       	        'WScript.Echo array_(j).Name
            End If
        Next
	'WScript.Echo array_(i).Name
    Next
    Set SortArrayList_ByName = array_
End Function

'Clean characters from string, that might cause problems when executing SAS code
Function strClean(inString)
  dim outString
  dim thisChar
  dim i
  outString=""
  for i=1 to len(inString)
    thisChar=mid(inString,i,1)
    Select Case thisChar
      Case ";", "'", """", "&", "%", "*"
        thisChar=" "
    end Select
    outString=outString+thisChar
   next
   strClean=outString
End Function

