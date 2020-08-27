using System.Windows;

namespace ProjectTemplateWizard.Views.Regions
{
    internal partial class ModuleEditingRegion
    {
        public ModuleEditingRegion()
        {
            InitializeComponent();
            ModuleNameTextBox.Loaded += ModuleNameTextBoxOnLoaded;
        }

        private void ModuleNameTextBoxOnLoaded(object sender, RoutedEventArgs e)
        {
            ModuleNameTextBox.CaretIndex = ModuleNameTextBox.Text.Length;
            ModuleNameTextBox.Focus();
        }
    }
}