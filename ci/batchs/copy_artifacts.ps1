$ErrorActionPreference = "Stop"
 
mkdir "debug";
copy-item "out\_debug\*\*" -destination "debug\" -recurse;
$archive = "senticode.xamarin.tools-$env:ASSEMBLY_VERSION-unsigned.zip"
compress-archive -path debug\* -destinationpath $archive
$debugdir = "d:\builddropfolder\senticode.xamarin.tools\builds\$env:BUILD_TYPE\Debug\$env:ASSEMBLY_VERSION"
mkdir $debugdir
copy-item $archive -destination $debugdir

mkdir "release-dev";
copy-item "out\_release\*\*" -destination "release-dev" -recurse;
$archive = "senticode.xamarin.tools-$env:ASSEMBLY_VERSION-signed-dev.zip"
compress-archive -path release-dev\* -destinationpath $archive
$releasedir = "d:\builddropfolder\senticode.xamarin.tools\builds\$env:BUILD_TYPE\Release\$env:ASSEMBLY_VERSION"
mkdir $releasedir
copy-item $archive -destination $releasedir

mkdir "release";
copy-item "release-dev\*" -destination "release" -recurse -exclude "*.pdb";
$archive = "senticode.xamarin.tools-$env:ASSEMBLY_VERSION-signed.zip"
compress-archive -path release\* -destinationpath $archive
copy-item $archive -destination $releasedir