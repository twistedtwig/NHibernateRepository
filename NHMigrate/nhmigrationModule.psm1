

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
	ForEach ($item in get-project -all) 
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


function SetupMigrateEXEPath {
	$path = "E:\Work\nhibernateRepo\NHibernateRepository\NHMigrate\bin\Debug\NHMigrate.exe"
	$path
	return 
}

function ProcessCommandLineArgs ($argList) {
	$result = ""
	
	foreach($arg in $argList) {
		$result = $result + " " + $arg
	}
	$result.trim()
	return
}

function runExe ($exePath, $argumentString) {

	$psi = New-object System.Diagnostics.ProcessStartInfo 
	$psi.CreateNoWindow = $true 
	$psi.UseShellExecute = $false 
	$psi.RedirectStandardOutput = $true 
	$psi.RedirectStandardError = $true 
	$psi.FileName = "$exePath"
	$psi.Arguments = "$argumentString"
	$process = New-Object System.Diagnostics.Process 
	$process.StartInfo = $psi 
	[void]$process.Start()
	$output = $process.StandardOutput.ReadToEnd() 
	$stderr = $process.StandardError.ReadToEnd() 
	$process.WaitForExit() 
	
	Write-Host "$output"
	Write-Host "ERRORS: $stderr"
	Write-Host "exit code: " + $process.ExitCode
}



#### #### #### #### #### #### #### #### #### #### #### #### #### #### 


function Enable-NHMigrations ($args) {
	write-host("Commencing Enable-Migrations")
	
	$exePath = SetupMigrateEXEPath

	$project = Get-Project
	$projPath = $project.fullName
	$otherArgs = ProcessCommandLineArgs $args

	$startupProject = FindStartUpProject
	$startupConfigPath = FindConfigFile $startupProject

	$argString = "Enable-Migrations " + $projPath + " " + $otherArgs + " -configFile " + $startupConfigPath
	
	runExe $exePath $argString

	
}


function Add-NHMigration {
	write-host("Commencing Add-Migration")
}

function Update-NHMigrations {
	write-host("Commencing update-Migrations")
}


#### #### #### #### #### #### #### #### #### #### #### #### #### #### 

Export-ModuleMember -Function 'Enable-NH*'
Export-ModuleMember -Function 'Add-NH*'
Export-ModuleMember -Function 'Update-NH*'