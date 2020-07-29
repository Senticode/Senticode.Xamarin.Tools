using System;
using ProjectTemplateWizard.Abstractions.Interfaces;
using ProjectTemplateWizard.ViewModels.Main;

namespace ProjectTemplateWizard.Views
{
    public partial class ProjectTemplateDialog
    {
        public ProjectTemplateDialog()
        {
            InitializeComponent();
        }

        public IProjectTemplateData ProjectTemplateData { get; internal set; }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            DataContext = new MainViewModel();
        }
    }
}