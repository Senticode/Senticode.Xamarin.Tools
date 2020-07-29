using System.Collections.Generic;
using EnvDTE80;
using EnvDTE90;
using ProjectTemplateWizard.Abstractions.Interfaces;

namespace SenticodeTemplate.Services
{
    internal class ProjectSettings
    {
        public IProjectTemplateData ProjectTemplateData { get; private set; }
        public string SavedPath { get; private set; }
        public string SavedProjectName { get; private set; }
        public Solution3 Solution { get; private set; }

        public void Init(IProjectTemplateData projectTemplateData, object automationObject,
            Dictionary<string, string> replacementsDictionary)
        {
            if (automationObject is DTE2 dte2 && dte2.Solution is Solution3 solution3)
            {
                Solution = solution3;
            }

            ProjectTemplateData = projectTemplateData;
            SavedPath = replacementsDictionary[AppConstants.SolutionDirectoryToken];
            SavedProjectName = replacementsDictionary[AppConstants.SafeProjectNameToken];
        }

        #region singleton

        private ProjectSettings()
        {
        }

        public static ProjectSettings Instance { get; } = new ProjectSettings();

        #endregion
    }
}