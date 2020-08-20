using System.IO;
using ProjectTemplateWizard.Abstractions.Interfaces;
using SenticodeTemplate.Constants;
using SenticodeTemplate.Interfaces;
using SenticodeTemplate.Services.Helpers;

namespace SenticodeTemplate.Services.Managers
{
    internal class SolutionwideProjectManager : IProjectManager
    {
        public void Compose()
        {
            var settings = ProjectSettings.Instance;
            var data = settings.ProjectTemplateData;

            if (data.IsNUnitIncluded)
            {
                SolutionwideProjectHelper.AddTestProjects(settings);
            }

            if (data.IsVersioningSystemIncluded)
            {
                SolutionwideProjectHelper.IncludeVersioningSystem(settings);
            }
        }

        private static class SolutionwideProjectHelper
        {
            private static void AddProject(ProjectSettings settings, string folderName, string templateName,
                string projectName)
            {
                var templatePath = settings.Solution.GetProjectTemplate($"{templateName}.zip", "CSharp");
                var projectPath = Path.Combine(settings.SavedPath, AppConstants.Src, folderName, projectName);
                var solutionFolder = ProjectHelper.AddSolutionFolder(settings.Solution, folderName);
                ProjectHelper.AddProjectToSolutionFolder(solutionFolder, templatePath, projectPath, projectName);
            }

            public static void AddTestProjects(ProjectSettings settings)
            {
                var data = settings.ProjectTemplateData;
                foreach (var module in data.CustomModules)
                {
                    AddProject(settings, AppConstants.Tests, AppConstants.TestProjectTemplateName,
                        $"{settings.SavedProjectName}.{module.ModuleType}.{module.Name}.{AppConstants.Tests}");
                }

                if (data.IsWebBackendIncluded)
                {
                    AddProject(settings, AppConstants.Tests, AppConstants.TestProjectTemplateName,
                        $"{settings.SavedProjectName}.{ModuleType.Xamarin}.{AppConstants.WebClientModule}.{AppConstants.Tests}");
                }

                if (data.XamarinDatabaseInfrastructureType != XamarinDatabaseInfrastructureType.None)
                {
                    AddProject(settings, AppConstants.Tests, AppConstants.TestProjectTemplateName,
                        $"{settings.SavedProjectName}.{ModuleType.Xamarin}.{AppConstants.DataAccessXamarinModule}.{AppConstants.Tests}");
                }

                if (data.WebDatabaseInfrastructureType != WebDatabaseInfrastructureType.None)
                {
                    AddProject(settings, AppConstants.Tests, AppConstants.TestProjectTemplateName,
                        $"{settings.SavedProjectName}.{ModuleType.Web}.{AppConstants.DataAccessWebModule}.{AppConstants.Tests}");
                }
            }

            public static void IncludeVersioningSystem(ProjectSettings settings)
            {
                var path = Path.Combine(settings.SavedPath, AppConstants.Sln, "SharedAssemblyInfo.cs");
                Directory.CreateDirectory(Path.Combine(settings.SavedPath, AppConstants.Sln));
                var stream = File.Create(path);
                stream.Close();
                FileHelper.ReplaceText("SenticodeTemplate.TemplateFiles.AssemblyInfo.template", path);

                var projects = Directory.GetFiles(settings.SavedPath, "*.csproj", SearchOption.AllDirectories);

                foreach (var project in projects)
                {
                    AddAssemblyInfoToProjectFiles(project);
                }
            }

            private static void AddAssemblyInfoToProjectFiles(string project)
            {
                string folders;

                //this is web api project and output path is set to default bin folder for docker support
                if (project.Contains($"{AppConstants.Web}.{AppConstants.Api}"))
                {
                    folders = "..\\..\\..\\";
                }
                else
                {
                    var output = FileHelper.FindStringStartingWith(project, "<OutputPath>");
                    output = output.Substring(output.IndexOf('>')+1);
                    folders = output.Remove(output.IndexOf('o'));
                }

                var assemblyInfoLink = AppConstants.AssemblyInfoLink.Replace("{folders}", folders);
                FileHelper.InsertString(project, "</PropertyGroup>", assemblyInfoLink);
            }
        }

        #region singleton

        private SolutionwideProjectManager()
        {
        }

        public static SolutionwideProjectManager Instance { get; } = new SolutionwideProjectManager();

        #endregion
    }
}
