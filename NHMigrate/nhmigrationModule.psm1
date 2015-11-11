$debug = $false

function FindConfigFile ($project){
	ForEach ($item in $project.ProjectItems | Where { $_.Name -eq "App.config"  -or $_.Name -eq "Web.config"}) 
	{ 		
		$configName = $item.FileNames(1)

		if($configName -ne $null -and $configName -ne ""){
			"""$configName"""
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
	$repoBinFolderPath = GetRepoBinFolderBinPath
	[io.path]::combine($repoBinFolderPath, 'NHMigrate.exe')

	"""$path"""
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


function GetNugetPackageNetFolder {

	$currentLoc = Get-Location
	$loc = $currentLoc.Path
	
	$packagesFolder = Get-ChildItem $loc -recurse | Where-Object {$_.PSIsContainer -eq $true -and $_.Name -match "packages"}
	$nhmigrateFolder = Get-ChildItem $packagesFolder -recurse | Where-Object {$_.PSIsContainer -eq $true -and $_.Name -like "NHibernateRepo.*"}
	$packageFolder = $nhmigrateFolder[0].FullName
	$netPackageFolder = [io.path]::combine($packageFolder, 'lib\net45')
	$netPackageFolder
}

function GetRepoBinFolderBinPath {
	$configStr = $dte.solution.properties.Item("ActiveConfig").value
	LogMessage "configStr $configStr"

	$config = $configStr.subString(0, $configStr.LastIndexOf("|"))

	$nugetPackageFolder = GetNugetPackageNetFolder

	$project = Get-Project
	$currentProjectPath = $project.FullName
	$currentProjectFolderPath = $currentProjectPath.subString(0, $currentProjectPath.LastIndexOf("\"))

	$outputPath = [io.path]::combine($currentProjectFolderPath, 'bin', $config)
	$outputPath
}


function copyExeToProjectFolder {
	$filesToCopy = "DatabaseManagement.dll", "DatabaseManagement.pdb", "Microsoft.Build.Framework.dll", "Microsoft.Build.Utilities.v4.0.dll", "NHMigrate.exe", "NHMigrate.pdb"

	$numgetFolder = GetNugetPackageNetFolder
	$repoBinPath = GetRepoBinFolderBinPath

	for ($i=0; $i -lt $filesToCopy.length; $i++) {
		$sourcePath = [io.path]::combine($numgetFolder, $filesToCopy[$i])
		Copy-Item $sourcePath repoBinPath		
	}
}

function SetupDebug ($args){
	if($args.Contains("-debug")){
		$debug = $true
		write-host("Debug enabled extra logging enabled")
	}
}

function LogMessage ($message) {
	if($debug) {
		write-host $message
	}
}

#### #### #### #### #### #### #### #### #### #### #### #### #### #### 


function Enable-NHMigrations ($args) {
	write-host("Commencing Enable-Migrations")
	SetupDebug $args

	copyExeToProjectFolder
	
	$exePath = SetupMigrateEXEPath

	$project = Get-Project
	$projPath = $project.fullName
	$otherArgs = ProcessCommandLineArgs $args

	$startupProject = FindStartUpProject
	$startupConfigPath = FindConfigFile $startupProject

	$argString = "Enable-Migrations " + """$projPath""" + " " + $otherArgs + " -configFile " + $startupConfigPath
	
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