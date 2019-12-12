$ErrorActionPreference = "Stop"
$env:GIT_REDIRECT_STDERR = '2>&1' # ignores error messages from "git checkout" and etc.

$message = "Version updated to $env:ASSEMBLY_VERSION"
git add sln\SharedAssemblyInfo.cs
git commit -m $message
git push