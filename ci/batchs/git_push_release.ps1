$ErrorActionPreference = "Stop"
$env:GIT_REDIRECT_STDERR = '2>&1' # ignores error messages from "git checkout" and etc.

git add sln\SharedAssemblyInfo.cs
git add src\Senticode.XFT.MVVM\Senticode.Xamarin.Tools.MVVM.nuspec
git add src\Senticode.XFT.Core\Senticode.Xamarin.Tools.Core.nuspec
$release_branch = "release-$env:ASSEMBLY_VERSION"
$message = "Version updated to $env:ASSEMBLY_VERSION"
git commit -m $message
git push --set-upstream origin $release_branch
git checkout dev
git merge $release_branch --no-ff
git push
git checkout master
git merge $release_branch --no-ff
git push
git push origin --delete $release_branch