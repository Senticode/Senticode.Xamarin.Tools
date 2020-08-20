using System.IO;
using SenticodeTemplate.Constants;
using SenticodeTemplate.Interfaces;
using SenticodeTemplate.Services.Helpers;

namespace SenticodeTemplate.Services.Managers
{
    internal class CommonProjectManager : IProjectManager
    {
        public void Compose()
        {
            var settings = ProjectSettings.Instance;
            var data = settings.ProjectTemplateData;
            CommonProjectHelper.AddEntitiesProject(settings);

            if (data.IsWebBackendIncluded)
            {
                CommonProjectHelper.AddWebInfrastructureProject(settings);
            }

            if (data.IsReadmeIncluded)
            {
                CommonProjectHelper.IncludeReadme(settings);
            }
        }

        private static class CommonProjectHelper
        {
            internal static void AddEntitiesProject(ProjectSettings settings)
            {
                var projectName = $"{settings.SavedProjectName}.{AppConstants.Common}.{AppConstants.Entities}";
                AddProject(settings, AppConstants.Common, AppConstants.CommonEntitiesTemplateName, projectName);
            }

            private static void AddProject(ProjectSettings settings, string folderName, string templateName,
                string projectName)
            {
                var templatePath = settings.Solution.GetProjectTemplate($"{templateName}.zip", "CSharp");
                var projectPath = Path.Combine(settings.SavedPath, AppConstants.Src, folderName, projectName);
                var solutionFolder = ProjectHelper.AddSolutionFolder(settings.Solution, folderName);
                ProjectHelper.AddProjectToSolutionFolder(solutionFolder, templatePath, projectPath, projectName);
            }

            public static void AddWebInfrastructureProject(ProjectSettings settings)
            {
                var projectName =
                    $"{settings.SavedProjectName}.{AppConstants.Common}.{AppConstants.Web}.{AppConstants.Infrastructure}";

                AddProject(settings, AppConstants.Common, AppConstants.WebInfrastructureTemplateName, projectName);
            }

            public static void IncludeReadme(ProjectSettings settings)
            {
                var path = Path.Combine(settings.SavedPath, "README.md");
                var stream = File.Create(path);
                stream.Close();
                FileHelper.ReplaceText("SenticodeTemplate.TemplateFiles.README.md", path);
            }
        }

        #region singleton

        private CommonProjectManager()
        {
        }

        public static CommonProjectManager Instance { get; } = new CommonProjectManager();

        #endregion
    }
}