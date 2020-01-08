$ErrorActionPreference = "Stop"   # errors should fail script
$c = get-content -path sln\BaseAssemblyInfo.cs | select-string -pattern '.*?(\d*\.\d*\.\d*\.\d*).*'
$assembly_version = $c.matches[0].groups[1].value
"ASSEMBLY_VERSION=$assembly_version" | Out-File env.properties -Encoding ASCII  # prop files - w/o spaces
echo "Creating environment variable for assembly version.."