using System;
using System.Text.RegularExpressions;
using System.Windows;
using ProjectTemplateWizard.Abstractions.Interfaces;
using ProjectTemplateWizard.ViewModels;

namespace ProjectTemplateWizard.ExtensionMethods
{
    internal static class ModuleEditingViewModelExtension
    {
        private static readonly Regex ModuleNameRegex = new Regex(@"^[a-zA-Z]+(\.[a-zA-Z]+)*$");

        public static bool Validate(this ModuleEditingViewModel viewModel)
        {
            var moduleName = viewModel.Name?.Trim();

            if (string.IsNullOrEmpty(moduleName) || !ModuleNameRegex.IsMatch(moduleName))
            {
                MessageBox.Show(
                    "Module name is not correct.\nIt should contain lettered-words divided with dots:\n...subSubName.subName.name",
                    "Save module", MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return false;
            }

            viewModel.Name = moduleName;

            return true;
        }

        public static void Clear(this ModuleEditingViewModel viewModel)
        {
            viewModel.Name = null;
            viewModel.IsWebModule = false;
            viewModel.IsXamarinModule = true;
            viewModel.SaveCommand = null;
            viewModel.SelectedModule = null;
            viewModel.EditingProcessType = ModuleEditingProcessType.None;
        }

        public static void CopyTo(this ModuleEditingViewModel source, ModuleViewModel target)
        {
            target.Name = source.Name;
            target.ModuleType = source.GetModuleType();
        }

        private static ModuleType GetModuleType(this ModuleEditingViewModel viewModel)
        {
            ModuleType moduleType;

            if (viewModel.IsWebModule)
            {
                moduleType = ModuleType.Web;
            }
            else if (viewModel.IsXamarinModule)
            {
                moduleType = ModuleType.Xamarin;
            }
            else
            {
                throw new NotSupportedException();
            }

            return moduleType;
        }

        public static ModuleViewModel ToModuleViewModel(this ModuleEditingViewModel viewModel)
        {
            var moduleViewModel = new ModuleViewModel();
            viewModel.CopyTo(moduleViewModel);

            return moduleViewModel;
        }
    }
}