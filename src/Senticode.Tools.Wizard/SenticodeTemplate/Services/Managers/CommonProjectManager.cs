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
            AddEntitiesProject(settings);
            AddWebInfrastructureProject(settings);
            AddReadme(settings);
        }

        private static void AddEntitiesProject(ProjectSettings settings)
        {
            var projectName = $"{settings.SavedProjectName}.{StringLiterals.Common}.{StringLiterals.Entities}";
            AddProject(settings, StringLiterals.Common, TemplateProjectNames.CommonEntities, projectName);
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

        private static void AddWebInfrastructureProject(ProjectSettings settings)
        {
            if (!settings.ProjectTemplateData.IsWebBackendIncluded)
            {
                return;
            }

            var projectName =
                $"{settings.SavedProjectName}.{StringLiterals.Common}.{StringLiterals.Web}.{StringLiterals.Infrastructure}";

            AddProject(settings, StringLiterals.Common, TemplateProjectNames.WebInfrastructure, projectName);
        }

        private static void AddReadme(ProjectSettings settings)
        {
            if (!settings.ProjectTemplateData.IsReadmeIncluded)
            {
                return;
            }

            var path = Path.Combine(settings.SavedPath, FileNames.ReadmeMd);

            using (File.Create(path))
            {
            }

            FileHelper.ReplaceText(FileNames.ReadmeTemplate, path);
        }

        #region singleton

        private CommonProjectManager()
        {
        }

        public static CommonProjectManager Instance { get; } = new CommonProjectManager();

        #endregion
    }
}