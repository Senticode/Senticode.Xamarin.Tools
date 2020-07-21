$ErrorActionPreference = "Stop"
$env:GIT_REDIRECT_STDERR = '2>&1' # ignores error messages from "git checkout" and etc.

$message = "Version updated to $env:ASSEMBLY_VERSION"
git add sln\SharedAssemblyInfo.cs
git add sln\BaseAssemblyInfo.cs
git add src\Senticode.XFT.MVVM\Senticode.Xamarin.Tools.MVVM.nuspec
git add src\Senticode.XFT.Core\Senticode.Xamarin.Tools.Core.nuspec
git commit -m $message
git push

$new_tag = "version-$env:ASSEMBLY_VERSION"
git tag $new_tag
git push origin $new_tag