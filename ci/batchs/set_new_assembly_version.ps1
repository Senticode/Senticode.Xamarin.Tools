$ErrorActionPreference = "Stop" # errors should fail script
$env:GIT_REDIRECT_STDERR = '2>&1' # ignores error messages from "git checkout" and etc.

$ver = $env:version | select-string -pattern '.*?(\d*\.\d*\.\d*\.).*'
if($ver.matches -eq $null) 
{
	throw "Wrong version format"
}
else
{	
	$full_version = "$($ver.matches.groups[1].value)1"
	"ASSEMBLY_VERSION=$full_version" |Out-File env.properties -Encoding ASCII;
	"BUILD_TYPE=release" |Out-File env.properties -Encoding ASCII -Append;
	echo "Created environment variable for assembly version"
	$release_branch = "release-$full_version"
	git checkout -b $release_branch
}