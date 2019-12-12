$ErrorActionPreference = "Stop"   # errors should fail script
$env:GIT_REDIRECT_STDERR = '2>&1' # ignores error messages from "git checkout" and etc.

git checkout dev

$c = get-content -path sln\SharedAssemblyInfo.cs | select-string -pattern '.*?(\d*\.\d*\.\d*\.).*'
$d = get-content -path sln\SharedAssemblyInfo.cs | select-string -pattern '.*?\d*\.\d*\.\d*\.(\d*).*'

$assembly_version = $c.matches[0].groups[1].value
$build_number = [int]$d.matches[0].groups[1].value + 1
$full_version = $assembly_version + $build_number 

echo "New build number: $full_version"

"ASSEMBLY_VERSION=$full_version" | Out-File env.properties -Encoding ASCII  # prop files - w/o spaces
"BUILD_TYPE=daily" | Out-File env.properties -Encoding ASCII -Append

echo "Creating environment variable for assembly version.."