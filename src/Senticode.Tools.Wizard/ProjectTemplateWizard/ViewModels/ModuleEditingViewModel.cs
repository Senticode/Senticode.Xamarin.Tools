using ProjectTemplateWizard.Abstractions;
using ProjectTemplateWizard.Core;

namespace ProjectTemplateWizard.ViewModels
{
    internal class ModuleEditingViewModel : ObservableObjectBase
    {
        public ModuleViewModel SelectedModule { get; set; }

        #region EditingProcessType: ModuleEditingProcessType

        public ModuleEditingProcessType EditingProcessType
        {
            get => _editingProcessType;
            set => SetProperty(ref _editingProcessType, value);
        }

        private ModuleEditingProcessType _editingProcessType;

        #endregion

        #region SaveCommand: Command

        public Command SaveCommand
        {
            get => _saveCommand;
            set => SetProperty(ref _saveCommand, value);
        }

        private Command _saveCommand;

        #endregion

        #region Name: string

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _name;

        #endregion

        #region IsXamarinModule: bool

        public bool IsXamarinModule
        {
            get => _isXamarinModule;
            set => SetProperty(ref _isXamarinModule, value);
        }

        private bool _isXamarinModule;

        #endregion

        #region IsWebModule: bool

        public bool IsWebModule
        {
            get => _isWebModule;
            set => SetProperty(ref _isWebModule, value);
        }

        private bool _isWebModule;

        #endregion
    }

    internal enum ModuleEditingProcessType
    {
        None,
        Edit,
        Add
    }
}