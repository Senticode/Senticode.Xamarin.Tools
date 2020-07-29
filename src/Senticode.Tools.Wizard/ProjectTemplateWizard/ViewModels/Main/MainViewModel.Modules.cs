using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ProjectTemplateWizard.ViewModels.Main
{
    internal partial class MainViewModel
    {
        public ObservableCollection<ModuleViewModel> Modules { get; private set; }
        public IReadOnlyCollection<SpecificModuleViewModel> SpecialModules { get; private set; }
        public SpecificModuleViewModel SignalRModule { get; private set; }
        public SpecificModuleViewModel WebBackendModule { get; private set; }
        public SpecificModuleViewModel DatabaseXamarinModule { get; private set; }
        public SpecificModuleViewModel DatabaseWebModule { get; private set; }

        private void InitializeModules()
        {
            SignalRModule = new SpecificModuleViewModel
            {
                IsWebModule = true,
                Name = "SignalRModule"
            };

            WebBackendModule = new SpecificModuleViewModel
            {
                IsWebModule = true,
                Name = "WebApiClientModule"
            };

            DatabaseXamarinModule = new SpecificModuleViewModel
            {
                IsXamarinModule = true,
                Name = "DataAccessXamarinModule"
            };

            DatabaseWebModule = new SpecificModuleViewModel
            {
                IsWebModule = true,
                Name = "DataAccessWebModule"
            };

            SpecialModules = new List<SpecificModuleViewModel>
            {
                WebBackendModule, SignalRModule, DatabaseWebModule, DatabaseXamarinModule
            };

            Modules = new ObservableCollection<ModuleViewModel>();
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