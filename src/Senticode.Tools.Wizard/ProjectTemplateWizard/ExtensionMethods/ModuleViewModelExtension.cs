using ProjectTemplateWizard.Abstractions.Interfaces;
using ProjectTemplateWizard.ViewModels;

namespace ProjectTemplateWizard.ExtensionMethods
{
    internal static class ModuleViewModelExtension
    {
        public static ModuleInfo ToModelInfo(this ModuleViewModel viewModel) =>
            new ModuleInfo(viewModel.Name, viewModel.ModuleType);

        public static void CopyTo(this ModuleViewModel source, ModuleEditingViewModel target)
        {
            target.Name = source.Name;

            switch (source.ModuleType)
            {
                case ModuleType.Web:
                    target.IsWebModule = true;
                    break;

                case ModuleType.Xamarin:
                    target.IsXamarinModule = true;
                    break;
            }
        }
    }
}