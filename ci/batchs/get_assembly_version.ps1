# $ErrorActionPreference = "Stop"   # errors should fail script
# $env:GIT_REDIRECT_STDERR = '2>&1' # ignores error messages from "git checkout" and etc.

git checkout dev

$base_changed=0

$last_tag = git describe --abbrev=0 --tags
$changes = git diff --name-only $last_tag HEAD
foreach($change in $changes)
{
 if($change -match ".*Senticode\.Base*" -and $change -notmatch ".*AssemblyInfo.*")
 {
    $base_changed=1
    break
 }
}
 
$c = get-content -path sln\SharedAssemblyInfo.cs | select-string -pattern '.*?(\d*\.\d*\.\d*\.).*'
$d = get-content -path sln\SharedAssemblyInfo.cs | select-string -pattern '.*?\d*\.\d*\.\d*\.(\d*).*'

$assembly_version = $c.matches[0].groups[1].value
$build_number = [int]$d.matches[0].groups[1].value + 1
$full_version = $assembly_version + $build_number 


$c = get-content -path sln\BaseAssemblyInfo.cs | select-string -pattern '.*?(\d*\.\d*\.\d*\.).*'
$d = get-content -path sln\BaseAssemblyInfo.cs | select-string -pattern '.*?\d*\.\d*\.\d*\.(\d*).*'
$assembly_version = $c.matches[0].groups[1].value
if ($base_changed -eq 1)
{
	$build_number = [int]$d.matches[0].groups[1].value + 1	 
}
else
{
	$build_number = $d.matches[0].groups[1].value
}
$base_version = $assembly_version + $build_number
	
@"
ASSEMBLY_VERSION=$full_version
BUILD_TYPE=daily
BASE_VERSION=$base_version
"@ | Out-File env.properties -Encoding ASCII 
