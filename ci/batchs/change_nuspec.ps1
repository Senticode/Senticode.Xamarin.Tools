$file = "src\Senticode.XFT.MVVM\Senticode.Xamarin.Tools.MVVM.nuspec"
$xmldata = [xml](Get-Content $file)
$xmldata.package.metadata.dependencies.dependency.SetAttribute("version", "$env:ASSEMBLY_VERSION")
$xmldata.Save($file)

$file = "src\Senticode.XFT.Core\Senticode.Xamarin.Tools.Core.nuspec"
$xmldata = [xml](Get-Content $file)
$xmldata.package.metadata.dependencies.dependency.SetAttribute("version", "$env:ASSEMBLY_VERSION")
$xmldata.Save($file)