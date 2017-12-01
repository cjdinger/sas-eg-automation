# -------------------------------------------
# DescribeEG43Project.ps1
# Purpose: read an EG project file and report the contents
# by using PowerShell objects
# Example uses:
#
#   DescribeEG43Project.ps1 c:\datasources\DonorsChoose.egp
#     - reports project contents to console or stdout
#
#   DescribeEG43Project.ps1 c:\datasources\DonorsChoose.egp | Out-GridView
#     - reports project contents into the PowerShell grid viewer
#
#   DescribeEG43Project.ps1 c:\datasources\DonorsChoose.egp | 
#      Export-CSV -Path c:\out\out.csv -NoTypeInformation
#
#     - reports project contents into a CSV file
#
#   DescribeEG43Project.ps1 C:\DataSources\DonorsChoose.egp |
#      Where-Object -FilterScript {$_."Input data" -ne "N/A"}
#
#   - report on only the tasks that have input data
# -------------------------------------------

# check for an input file
if ($args.Count -eq 1)
{
  $fileToProcess = $args[0] 
}
else
{
  Write-Host "EXAMPLE Usage: DescribeEGProject.ps1 path-and-name-EGP-file"
  Exit -1
}

# check that the input file exists
if (-not (Test-Path $fileToProcess))
{
  Write-Host "$fileToProcess does not exist."
  Exit -1
}
# change this if running a different version of EG
$egVersion = "SASEGObjectModel.Application.7.1" 

# create an instance of the EG automation model
$eguideApp = New-Object -comObject $egVersion
$project = $eguideApp.Open("$fileToProcess", "")

# Show all of the process flows in the project
$pfCollection = $project.ContainerCollection
foreach ($pf in $pfCollection)
{
  if ($pf.ContainerType -eq 0)
  {
	foreach ($item in $pf.Items)
	{
        $objRecord = New-Object psobject
		
		# add static properties for each record
		$objRecord | add-member noteproperty `
		 -name "ProcessFlow" `
		 -value  $pf.Name;

		$objRecord | add-member noteproperty `
		 -name "ItemType" `
         -value "Unknown"
		 
 		$objRecord | add-member noteproperty `
		 -name "Label"  `
		 -value $item.Name

 		$objRecord | add-member noteproperty `
		 -name "Input data"  `
		 -value "N/A"
		 
	  # report on each item within the process flow
	  switch ($item.GetType().ToString())
	  {
	    "SAS.EG.Scripting.Data" 
		{
 		    $objRecord.ItemType = "Data" 
			foreach ($task in $item.Tasks)
			{
				$objTaskRecord = New-Object psobject
				
				# add static properties for each record
				$objTaskRecord | add-member noteproperty `
				 -name "ProcessFlow" `
				 -value  $pf.Name;

				$objTaskRecord | add-member noteproperty `
				 -name "ItemType" `
				 -value "Task"
				 
				$objTaskRecord | add-member noteproperty `
				 -name "Label"  `
				 -value $task.Name			   
				 
				$objTaskRecord | add-member noteproperty `
				 -name "Input data"  `
				 -value $item.Name		
				 
				 $objTaskRecord
			}
		}  
		"SAS.EG.Scripting.Link" { $objRecord.ItemType = "User link" }  
		"SAS.EG.Scripting.Code" { $objRecord.ItemType = "SAS program" }  
		"SAS.EG.Scripting.Query" { $objRecord.ItemType = "Query" }  
		"SAS.EG.Scripting.Task" { $objRecord.ItemType = "Task" }  		
	    "SAS.EG.Scripting.File" { $objRecord.ItemType = "External file" }  		
		"SAS.EG.Scripting.Report" { $objRecord.ItemType = "SAS report" }  
		"SAS.EG.Scripting.Cube" { $objRecord.ItemType = "OLAP cube" }  			
		default { $objRecord.ItemType = $item.GetType().ToString() }  
	   }
	  
	  $objRecord
	}	
  }
}
$project.Close()
$eguideApp.Quit()