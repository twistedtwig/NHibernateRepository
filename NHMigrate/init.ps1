param($installPath, $toolsPath, $package)

Import-Module (Join-Path $toolsPath nhmigration.psm1)

"install test" | out-file C:\temp\nhmigrateinstalltest.txt