using System.IO;
using System.Linq;
using EnvDTE;
using EnvDTE80;
using EnvDTE90;
using SenticodeTemplate.Constants;
using VSLangProj;

namespace SenticodeTemplate.Services.Helpers
{
    public static class ProjectHelper
    {
        public static Project AddSolutionFolder(Solution3 solution, string folderName)
        {
            var project = solution.Projects
                .Cast<Project>()
                .FirstOrDefault(x => x.Name == folderName);

            return project ?? solution.AddSolutionFolder(folderName);
        }

        public static Project AddSolutionFolder(Project solutionFolderProject, string folderName)
        {
            for (var i = 1; i < solutionFolderProject.ProjectItems.Count + 1; i++)
            {
                var subProject = solutionFolderProject.ProjectItems.Item(i).SubProject;

                if (subProject.Name == folderName)
                {
                    return subProject;
                }
            }

            var solutionFolder = (SolutionFolder) solutionFolderProject.Object;

            return solutionFolder.AddSolutionFolder(folderName);
        }

        public static Project GetProjectByName(Solution3 solution, string projectName, string containingFolderName,
            string outerFolder)
        {
            var project = solution.Projects
                .Cast<Project>()
                .FirstOrDefault(x => x.Name == outerFolder);

            if (project != null)
            {
                var folder = GetProjectByName(project, containingFolderName);

                if (folder != null)
                {
                    return GetProjectByName(folder, projectName);
                }
            }

            return null;
        }

        public static Project GetProjectByName(Solution3 solution, string projectName, string containingFolderName)
        {
            var project = solution.Projects
                .Cast<Project>()
                .FirstOrDefault(x => x.Name == containingFolderName);

            return project == null ? null : GetProjectByName(project, projectName);
        }

        public static Project GetProjectByName(Project project, string projectName)
        {
            for (var i = 1; i < project.ProjectItems.Count + 1; i++)
            {
                var subProject = project.ProjectItems.Item(i).SubProject;

                if (subProject.Name == projectName)
                {
                    return subProject;
                }
            }

            return null;
        }

        public static void AddProjectReference(Project project, Project referenceProject)
        {
            if (project.Object is VSProject proj)
            {
                proj.References.AddProject(referenceProject);
                project.Save();
            }
        }

        public static void AddProjectToSolutionFolder(Project solutionFolderProject, string templatePath,
            string projectPath, string projectName)
        {
            if (solutionFolderProject.Object is SolutionFolder solutionFolder)
            {
                solutionFolder.AddFromTemplate(templatePath, projectPath, projectName);
            }
        }

        public static string RenameModuleInitializer(string baseDirectory, string projectName, string moduleType,
            string moduleName)
        {
            var index = moduleName.LastIndexOf('.') + 1;
            var classname = moduleName.Substring(index, moduleName.Length - index);

            classname += moduleName.Contains(StringLiterals.Module)
                ? StringLiterals.Initializer
                : StringLiterals.ModuleInitializer;

            var path = Path.Combine(baseDirectory, StringLiterals.Src, moduleType, StringLiterals.Modules,
                $"{projectName}.{moduleName}", $"{StringLiterals.ModuleInitializer}.{FileExtensions.Cs}");

            FileHelper.ReplaceString(path,
                File.ReadAllText(path).Contains(ReplacementTokens.ModuleInitializer)
                    ? ReplacementTokens.ModuleInitializer
                    : StringLiterals.ModuleInitializer, classname);

            File.Move(path,
                path.Replace($"{StringLiterals.ModuleInitializer}.{FileExtensions.Cs}",
                    $"{classname}.{FileExtensions.Cs}"));

            return classname;
        }
    }
}