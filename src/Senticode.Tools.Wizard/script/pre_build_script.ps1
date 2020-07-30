param(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$BaseDirectory
)

$excludeFolders = @('bin', 'obj')

$projectFolders = @()
$templateFolders = @()

function AnalyzeFolder 
{    
    param($folder, $xmlnode)
    $items = Get-ChildItem $folder    

	foreach ($item in $items)
	{               
		if ((Get-Item $folder\$item) -is [System.IO.DirectoryInfo])
		{
			$childNode = $xmldata.CreateElement("Folder", $xmlNS)
            $childNode.SetAttribute("Name", $item)
            $childNode.SetAttribute("TargetFolderName", $item)
            $xmlnode.AppendChild($childNode)            
            
            AnalyzeFolder "$folder\$item" $childNode
            
		}
		else
		{
            if(($item -notmatch '.csproj$') -and
                ($item -notmatch '__TemplateIcon.ico') -and
                ($item -notmatch '.vstemplate'))
            {
                $childNode = $xmldata.CreateElement("ProjectItem", $xmlNS)
                if($item -match '.png$')
                {
                    $childNode.SetAttribute("ReplaceParameters", "false")
                }
                else
                {
                    $childNode.SetAttribute("ReplaceParameters", "true")
                }               
                $childNode.SetAttribute("TargetFileName", $item)
                $childNode.InnerText = $item
                $xmlnode.AppendChild($childNode)
            }			
		}
	}
	
}

function ReplaceStrings
{
    foreach ($templateFolder in $templateFolders)
    {    
		$projectFile = "$BaseDirectory\out\$templateFolder\$templateFolder.csproj"
		
        (Get-Content $projectFile -raw) -replace '<ItemGroup>.*\n(?:.*Template.*\n){1,}.*<\/ItemGroup>|<ItemGroup>.*\n.*Template.*\n.*\n.*Template.*\n.*\n.*<\/ItemGroup>', ' '| 
		Set-Content $projectFile
		
		(Get-Content $projectFile) | Foreach-Object {
		$_ -replace '..\\out\\_release\\templates', 'out\release' -replace '..\\out\\_debug\\templates', 'out\debug' -replace '..\\out\\_obj\\templates', 'out\obj' 
        }| Set-Content $projectFile
               
        $files = Get-ChildItem "$BaseDirectory\out\$templateFolder" -recurse -file -exclude *.vstemplate,*.ico,*.png       
        
        foreach ($file in $files)
        {                
            (Get-Content $file) -replace $templateFolder,'$safeprojectname$' |
            Set-Content $file
            
            if(($templateFolder -eq 'Template.Blank') -or ($templateFolder -eq 'Template.MasterDetail'))
            {
                (Get-Content $file) -replace 'Template\.','$safeprojectname$.' |
                Set-Content $file
            }
            else
            {
                (Get-Content $file) -replace '(Template\.MasterDetail|Template\.)','$namespace$.' |
                Set-Content $file
            }            

            if(($file -match '.Wpf.csproj') -or ($file -match '.GtkSharp.csproj'))
            {
                (Get-Content $file) -replace '\\..\\sln\\Wizard\\packages','\packages' |
                Set-Content $file
            }
            
        }        
    }
}

Write-Output "Searching for project template files...";

$files = Get-ChildItem "$BaseDirectory"
foreach($file in $files)   # find all folders that contain template projects
{
    if ($file -match 'Templates')
    {
        $innerFiles = Get-ChildItem "$BaseDirectory\$file\*" -recurse
        foreach($innerFile in $innerFiles)
        {
            if($innerFile -match '.csproj$')
            {			
               $projectFolders += $innerFile.DirectoryName
            }
        }
    }
}

Write-Output "Copying projects from template folders to dedicated folders...";
foreach ($projectFolder in $projectFolders) 
{
    $templateFolder = $projectFolder|split-path -leaf
    Remove-Item "$BaseDirectory\out\$templateFolder" -Recurse -ErrorAction Ignore
    mkdir "$BaseDirectory\out\$templateFolder" -force
    $templateFolders += $templateFolder
    
    Copy-Item "$BaseDirectory\script\__TemplateIcon.ico" "$BaseDirectory\out\$templateFolder" -force
    Copy-Item "$BaseDirectory\script\MyTemplate.vstemplate" "$BaseDirectory\out\$templateFolder" -force

    Get-ChildItem $projectFolder\* -directory | 
    Where-Object{$_.Name -notin $excludeFolders } | 
    Copy-Item -destination "$BaseDirectory\out\$templateFolder" -recurse -force

    Get-ChildItem $projectFolder\* -file | 
    Where-Object{$_.Name -notmatch '.csproj.user$' } |     
    Copy-Item -destination "$BaseDirectory\out\$templateFolder" -recurse -force  
    
    ReplaceStrings  
}

Write-Output "Creating .vstemplate files..."; 
foreach ($templateFolder in $templateFolders) 
{ 
    $file = "$BaseDirectory\out\$templateFolder\MyTemplate.vstemplate"
    $xmldata = [xml](Get-Content $file)
    $xmlNS = $xmldata.DocumentElement.NamespaceURI
    
    $xmldata.VSTemplate.TemplateData.Name =  $templateFolder.ToString()
    $xmldata.VSTemplate.TemplateData.DefaultName =  $templateFolder.ToString()
    
    $xmldata.VSTemplate.TemplateContent.Project.SetAttribute("TargetFileName", "$templateFolder.csproj")
    $xmldata.VSTemplate.TemplateContent.Project.SetAttribute("File", "$templateFolder.csproj")   
    
    AnalyzeFolder "$BaseDirectory\out\$templateFolder" $xmldata.VSTemplate.TemplateContent.Project
    
    $xmldata.Save("$file")
}

Write-Output "Archiving templates..."; 
foreach ($templateFolder in $templateFolders)
{         
    Compress-Archive -path "$BaseDirectory\out\$templateFolder" -destinationpath "$BaseDirectory\out\$templateFolder.zip" -force
	mkdir "$BaseDirectory\SenticodeTemplate\ProjectTemplates" -ErrorAction Ignore
    Move-Item "$BaseDirectory\out\$templateFolder.zip" "$BaseDirectory\SenticodeTemplate\ProjectTemplates" -force
}

$templateFiles = Get-ChildItem "$BaseDirectory\TemplateFiles" 
foreach ($folder in $templateFiles)
{    
    Compress-Archive -path "$BaseDirectory\TemplateFiles\$folder" -destinationpath "$BaseDirectory\out\$folder.zip" -force
    Move-Item "$BaseDirectory\out\$folder.zip" "$BaseDirectory\SenticodeTemplate\ProjectTemplates" -force
}