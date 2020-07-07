$nuget = resolve-path "$env:SENTICODE_CI_TOOLS\common\nuget\*\nuget.exe" | Select -ExpandProperty Path
& $nuget pack src\Senticode.XFT.MVVM\Senticode.Xamarin.Tools.MVVM.csproj -Version $env:ASSEMBLY_VERSION -Properties Configuration=Release -IncludeReferencedProjects -OutputDirectory .
& $nuget pack src\Senticode.XFT.UI\Senticode.Xamarin.Tools.UI.csproj -Version $env:ASSEMBLY_VERSION -Properties Configuration=Release -IncludeReferencedProjects -OutputDirectory .
& $nuget pack src\Senticode.Database.Tools\Senticode.Database.Tools.csproj -Version $env:ASSEMBLY_VERSION -Properties Configuration=Release -IncludeReferencedProjects -OutputDirectory .