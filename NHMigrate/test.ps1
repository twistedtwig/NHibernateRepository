
function GetSolutionPath
{
	$solFullPath = $dte.solution.properties.Item("path").value
	$solPath = $solFullPath.subString(0, $solFullPath.LastIndexOf("\")) + "\"

	$solPath
	return
}

function GetRelativeProjectPath($project, $solutionPath){

	$projectRelPath = $project.FullName.Replace($solutionPath, "")
	$projectRelPath = $dte.solution.properties.Item("Name").value + "\" + $projectRelPath.subString(0, $projectRelPath.LastIndexOf("\"))

	$projectRelPath 
	return
}

function ReloadProject ($projectPath) {
	
	$dte.Windows.Item("{3AE79031-E1BC-11D0-8F78-00A0C9110057}").Activate() 
	#$dte.ActiveWindow.Object | get-member
	$dte.ActiveWindow.Object.GetItem($projectPath).Select(1)
	$dte.ExecuteCommand("Project.UnloadProject")
	$dte.ExecuteCommand("Project.ReloadProject")
}

function RefreshScreen {
	$solPath = GetSolutionPath
	$project = Get-Project
	$projectRelPath = GetRelativeProjectPath $project  $solPath	
	ReloadProject $projectRelPath
}



function FindConfigFile ($project){
	ForEach ($item in $project.ProjectItems | Where { $_.Name -eq "App.config"  -or $_.Name -eq "Web.config"}) 
	{ 		
		$configName = $item.FileNames(1)

		if($configName -ne $null -and $configName -ne ""){
			$configName 
			return
		}
	} 
}


function FindStartUpProject {
	ForEach ($item in $dte.solution.projects) 
	{ 
		if($item.Kind -eq "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}" -or $item.Kind -eq "{F184B08F-C81C-45F6-A57F-5ABD9991F28F}")
		{
			if($item.Name -eq $dte.solution.properties.Item("startupproject").value)
			{
				$item
				return
			}

		}
	}
}



##################################################
##################################################
##################################################
##################################################
##################################################
##################################################


clear

#$solPath = GetSolutionPath
#$project = Get-Project
#write-host("selected project path")
#$projectRelPath = GetRelativeProjectPath $project  $solPath
#$projectRelPath

#ReloadProject $projectRelPath



#StartupObject


#write-host("config file for selected project")
#$confFile = FindConfigFile $project
#$confFile





function CopyMigrationToBin {
	$sp = FindStartUpProject
	$p = $sp.FullName
	$s = $p.subString(0, $p.LastIndexOf("\"))
	
	$configStr = $dte.solution.properties.Item("ActiveConfig").value
	$config = $configStr.subString(0, $configStr.LastIndexOf("|"))
	
	$path = [io.path]::combine($s, 'bin', $config)
	
	$currentLoc = Get-Location
	$loc = $currentLoc.Path

	$packagesFolder = Get-ChildItem $loc -recurse | Where-Object {$_.PSIsContainer -eq $true -and $_.Name -match "packages"}
	$nhmigrateFolder = Get-ChildItem $packagesFolder -recurse | Where-Object {$_.PSIsContainer -eq $true -and $_.Name -like "NHibernateRepo.*"}
	#$nhmigrateFolder = Get-ChildItem $packagesFolder -recurse | Where-Object {$_.PSIsContainer -eq $true -and $_.Name -like "NHMigrate.*"}
	
	if($nhmigrateFolder.length -ge 1){
		$pack = $nhmigrateFolder[$nhmigrateFolder.length-1]
		#copy nhmigrate.exe to $path
	}
	else{
		write-host "ERROR: Could not find NHMigrate"
	}
}


#CopyMigrationToBin




#$currentLoc | Get-Member


#write-host("config file for startup project")
#$confFile = FindConfigFile $sp
#$confFile




#import-module E:\Work\nhibernateRepo\NHibernateRepository\NHMigrate\nhmigrationModule.psm1
#enable-nhmigrations -repo exampleRepo -debug
#remove-module nhmigrationModule


function GetNugetPackageNetFolder {

	$currentLoc = Get-Location
	$loc = $currentLoc.Path

	$packagesFolder = Get-ChildItem $loc -recurse | Where-Object {$_.PSIsContainer -eq $true -and $_.Name -match "packages"}
	$nhmigrateFolder = Get-ChildItem $packagesFolder -recurse | Where-Object {$_.PSIsContainer -eq $true -and $_.Name -like "VSSDK.Shell.Interop.*"}
	$packageFolder = $nhmigrateFolder[0].FullName
	$netPackageFolder = [io.path]::combine($packageFolder, 'lib\net45')
	$netPackageFolder
}

function GetRepoBinFolderBinPath {
	$configStr = $dte.solution.properties.Item("ActiveConfig").value
	$config = $configStr.subString(0, $configStr.LastIndexOf("|"))

	$nugetPackageFolder = GetNugetPackageNetFolder

	$project = Get-Project
	$currentProjectPath = $project.FullName
	$currentProjectFolderPath = $currentProjectPath.subString(0, $currentProjectPath.LastIndexOf("\"))

	$outputPath = [io.path]::combine($currentProjectFolderPath, 'bin', $config)
	$outputPath
}

$filesToCopy = "DatabaseManagement.dll", "DatabaseManagement.pdb", "Microsoft.Build.Framework.dll", "Microsoft.Build.Utilities.v4.0.dll", "NHMigrate.exe", "NHMigrate.pdb"

$x = GetNugetPackageNetFolder
$x

$y = GetRepoBinFolderBinPath
$y

for ($i=0; $i -lt $filesToCopy.length; $i++) {
	$sourcePath = [io.path]::combine($x, $filesToCopy[$i])
	Copy-Item $sourcePath $y
	$filesToCopy[$i]
}