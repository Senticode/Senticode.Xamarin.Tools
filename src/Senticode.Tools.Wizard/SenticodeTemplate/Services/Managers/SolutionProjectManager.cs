using System.IO;
using ProjectTemplateWizard.Abstractions.Interfaces;
using SenticodeTemplate.Constants;
using SenticodeTemplate.Interfaces;
using SenticodeTemplate.Services.Helpers;

namespace SenticodeTemplate.Services.Managers
{
    internal class SolutionProjectManager : IProjectManager
    {
        public void Compose()
        {
            var settings = ProjectSettings.Instance;
            AddTestProjects(settings);
            IncludeVersioningSystem(settings);
        }

        private static void AddProject(ProjectSettings settings, string folderName, string templateName,
            string projectName)
        {
            var templatePath =
                settings.Solution.GetProjectTemplate($"{templateName}.{FileExtensions.Zip}", LanguageNames.CSharp);

            var projectPath = Path.Combine(settings.SavedPath, StringLiterals.Src, folderName, projectName);
            var solutionFolder = ProjectHelper.AddSolutionFolder(settings.Solution, folderName);

            ProjectHelper.AddProjectToSolutionFolder(solutionFolder, templatePath, projectPath, projectName);
        }

        private static void AddTestProjects(ProjectSettings settings)
        {
            var data = settings.ProjectTemplateData;

            if (!data.IsNUnitIncluded)
            {
                return;
            }

            foreach (var module in data.CustomModules)
            {
                AddProject(settings, StringLiterals.Tests, TemplateProjectNames.Test,
                    $"{settings.SavedProjectName}.{module.ModuleType}.{module.Name}.{StringLiterals.Tests}");
            }

            if (data.IsWebBackendIncluded)
            {
                AddProject(settings, StringLiterals.Tests, TemplateProjectNames.Test,
                    $"{settings.SavedProjectName}.{ModuleType.Xamarin}.{StringLiterals.WebClientModule}.{StringLiterals.Tests}");
            }

            if (data.XamarinDatabaseInfrastructureType != XamarinDatabaseInfrastructureType.None)
            {
                AddProject(settings, StringLiterals.Tests, TemplateProjectNames.Test,
                    $"{settings.SavedProjectName}.{ModuleType.Xamarin}.{StringLiterals.DataAccessXamarinModule}.{StringLiterals.Tests}");
            }

            if (data.WebDatabaseInfrastructureType != WebDatabaseInfrastructureType.None)
            {
                AddProject(settings, StringLiterals.Tests, TemplateProjectNames.Test,
                    $"{settings.SavedProjectName}.{ModuleType.Web}.{StringLiterals.DataAccessWebModule}.{StringLiterals.Tests}");
            }
        }

        private static void IncludeVersioningSystem(ProjectSettings settings)
        {
            if (!settings.ProjectTemplateData.IsVersioningSystemIncluded)
            {
                return;
            }

            var path = Path.Combine(settings.SavedPath, StringLiterals.Sln, FileNames.SharedAssemblyInfoCs);
            Directory.CreateDirectory(Path.Combine(settings.SavedPath, StringLiterals.Sln));

            using (File.Create(path))
            {
            }

            FileHelper.ReplaceText(FileNames.SenticodeTemplateTemplateFilesAssemblyInfoTemplate, path);
            var projects = Directory.GetFiles(settings.SavedPath, $"*.{FileExtensions.CsProj}",
                SearchOption.AllDirectories);

            foreach (var project in projects)
            {
                AddAssemblyInfoToProjectFiles(project);
            }
        }

        private static void AddAssemblyInfoToProjectFiles(string project)
        {
            string folders;

            // This is web api project and output path is set to default bin folder for docker support.
            if (project.Contains($"{StringLiterals.Web}.{StringLiterals.Api}"))
            {
                folders = "..\\..\\..\\";
            }
            else
            {
                var output = FileHelper.FindStringStartingWith(project, "<OutputPath>");
                output = output.Substring(output.IndexOf('>') + 1);
                folders = output.Remove(output.IndexOf('o'));
            }

            var assemblyInfoLink = CodeConstants.AssemblyInfoLink.Replace("{folders}", folders);
            FileHelper.InsertString(project, StringLiterals.PropertyGroupTag, assemblyInfoLink);
        }

        #region singleton

        private SolutionProjectManager()
        {
        }

        public static SolutionProjectManager Instance { get; } = new SolutionProjectManager();

        #endregion
    }
}