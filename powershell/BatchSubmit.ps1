# Script to automate local EG to read and run a local .SAS program 
# on a remote SAS session that EG can connect to
# Log file and HTML result will be saved in a local subfolder

# check for an input file
if ($args.Count -eq 1)
{
  $fileToProcess = $args[0] 
}
else
{
  Write-Host "EXAMPLE Usage: BatchSubmit.ps1 path-and-name-of-SAS-file"
  Exit -1
}

# check that the input file exists
if (-not (Test-Path $fileToProcess))
{
  Write-Host "$fileToProcess does not exist."
  Exit -1
}


# calculate a name for the log file
$fileName = Split-Path -Path $fileToProcess -Leaf
$file = Get-Item -Path $fileToProcess
$rootFileName = [System.IO.Path]::GetFileNameWithoutExtension($file.Name)
$logFileName = $rootFileName + ".log"

$currentDirectory = Get-Location

# Combine the current directory with the subfolder name to get the full path
$subfolderPath = Join-Path -Path $currentDirectory -ChildPath $rootFileName
$logFullPath = Join-Path -Path $subfolderPath -ChildPath $logFileName
$htmlFullPath = Join-Path -Path $subfolderPath -ChildPath ($rootFileName + ".html")

# Check if the subfolder exists
if (-Not (Test-Path -Path $subfolderPath)) {
    # Create the subfolder if it doesn't exist
    New-Item -Path $subfolderPath -ItemType Directory
    Write-Host "Subfolder '$subfolderName' created at $subfolderPath"
} else {
    Write-Host "Subfolder '$subfolderName' already exists at $subfolderPath"
}

# change this if running a different version of EG
$egVersion = "SASEGObjectModel.Application.8.1" 

# create an instance of the EG automation model
$eguideApp = New-Object -comObject $egVersion
Write-Host $eguideApp.Name  $eguideApp.Version

Write-Host "Opening " $fileToProcess "to run in batch"
$programText = Get-Content $fileToProcess
Write-Host $programText

# create a new code item to run in EG, within a new temp project
$project = $eguideApp.New()
$sasProgram = $project.CodeCollection.Add()

# override options to generate an HTML5 output
# if needed, set server
# $sasProgram.Server = "SASApp"
$sasProgram.UseApplicationOptions = 0
$sasProgram.GenListing = 0
$sasProgram.GenHTML = 1
$sasProgram.Text = $programText

# Run the code
$sasProgram.Run()

# Save the log and the HTML if available
$sasProgram.Log.SaveAs($logFullPath)
$sasProgram.Results.Item(0).SaveAs($htmlFullPath)

$project.Close()

# Quit (end) the application object
$eguideApp.Quit()