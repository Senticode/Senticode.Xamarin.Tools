using System;
using System.Windows;
using ProjectTemplateWizard.Views;

namespace ProjectTemplateWizard.QuickStart
{
    internal partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            var dialog = new ProjectTemplateDialog();
            dialog.ShowDialog();
            Application.Current.MainWindow?.Close();
        }
    }
}