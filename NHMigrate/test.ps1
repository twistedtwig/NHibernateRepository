
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






#$sp = FindStartUpProject
#$sp
#write-host("config file for startup project")
#$confFile = FindConfigFile $sp
#$confFile




import-module E:\Work\nhibernateRepo\NHibernateRepository\NHMigrate\nhmigrationModule.psm1
enable-nhmigrations -repo exampleRepo
remove-module nhmigrationModule



