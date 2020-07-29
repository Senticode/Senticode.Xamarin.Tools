using System.Collections.Generic;
using System.IO;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using ProjectTemplateWizard.Views;
using SenticodeTemplate.Services;

namespace SenticodeTemplate
{
    public class Wizard : IWizard
    {
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary,
            WizardRunKind runKind, object[] customParams)
        {
            var dialog = new ProjectTemplateDialog();
            dialog.ShowDialog();
            var data = dialog.ProjectTemplateData;

            if ((dialog.DialogResult ?? false) && data != null)
            {
                ProjectSettings.Instance.Init(data, automationObject, replacementsDictionary);
            }
            else
            {
                throw new WizardBackoutException();
            }
        }

        public void ProjectFinishedGenerating(Project project)
        {
            var emptyFolder = Path.Combine(ProjectSettings.Instance.SavedPath,
                ProjectSettings.Instance.SavedProjectName);

            if (Directory.Exists(emptyFolder))
            {
                Directory.Delete(emptyFolder);
            }
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            // Method intentionally left empty.
        }

        public bool ShouldAddProjectItem(string filePath) => true;

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
            // Method intentionally left empty.
        }

        public void RunFinished()
        {
            SolutionGenerator.Instance.Run();
        }
    }
}