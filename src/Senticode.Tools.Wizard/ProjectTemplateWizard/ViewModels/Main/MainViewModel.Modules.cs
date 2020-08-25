using System.Collections.Generic;
using System.Collections.ObjectModel;
using ProjectTemplateWizard.Abstractions.Interfaces;

namespace ProjectTemplateWizard.ViewModels.Main
{
    internal partial class MainViewModel
    {
        public ObservableCollection<ModuleViewModel> CustomModules { get; private set; }
        public IReadOnlyCollection<SpecificModuleViewModel> SpecialModules { get; private set; }
        public SpecificModuleViewModel SignalRModule { get; private set; }
        public SpecificModuleViewModel WebBackendModule { get; private set; }
        public SpecificModuleViewModel DatabaseXamarinModule { get; private set; }
        public SpecificModuleViewModel DatabaseWebModule { get; private set; }

        private void InitializeModules()
        {
            SignalRModule = new SpecificModuleViewModel
            {
                ModuleType = ModuleType.Web,
                Name = "SignalRModule"
            };

            WebBackendModule = new SpecificModuleViewModel
            {
                ModuleType = ModuleType.Web,
                Name = "WebApiClientModule"
            };

            DatabaseXamarinModule = new SpecificModuleViewModel
            {
                ModuleType = ModuleType.Xamarin,
                Name = "DataAccessXamarinModule"
            };

            DatabaseWebModule = new SpecificModuleViewModel
            {
                ModuleType = ModuleType.Web,
                Name = "DataAccessWebModule"
            };

            SpecialModules = new List<SpecificModuleViewModel>
            {
                WebBackendModule, SignalRModule, DatabaseWebModule, DatabaseXamarinModule
            };

            CustomModules = new ObservableCollection<ModuleViewModel>();
        }

        #region SelectedModule: ModuleViewModel

        public ModuleViewModel SelectedModule
        {
            get => _selectedModule;
            set => SetProperty(ref _selectedModule, value);
        }

        private ModuleViewModel _selectedModule;

        #endregion
    }
}