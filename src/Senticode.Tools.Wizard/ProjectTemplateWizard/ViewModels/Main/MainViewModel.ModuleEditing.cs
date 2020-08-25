using System;
using System.Diagnostics;
using ProjectTemplateWizard.Core;
using ProjectTemplateWizard.ExtensionMethods;

namespace ProjectTemplateWizard.ViewModels.Main
{
    internal partial class MainViewModel
    {
        public ModuleEditingViewModel ModuleEditingViewModel { get; private set; }

        public void InitializeModuleEditing()
        {
            ModuleEditingViewModel = new ModuleEditingViewModel {IsXamarinModule = true};
        }

        #region IsModuleEditModeOn: bool

        /// <summary>
        ///     Gets or sets the IsModuleEditModeOn value.
        /// </summary>
        public bool IsModuleEditModeOn
        {
            get => _isModuleEditModeOn;
            set => SetProperty(ref _isModuleEditModeOn, value, OnIsModuleEditModeOnChanged);
        }

        private void OnIsModuleEditModeOnChanged()
        {
            if (!IsModuleEditModeOn)
            {
                ModuleEditingViewModel.Clear();
            }
        }

        /// <summary>
        ///     IsModuleEditModeOn property data.
        /// </summary>
        private bool _isModuleEditModeOn;

        #endregion

        #region SaveEditModule command

        public Command SaveEditModuleCommand =>
            _saveEditModuleCommand ??= new Command(ExecuteSaveEditModule, () => true);

        private Command _saveEditModuleCommand;

        private void ExecuteSaveEditModule()
        {
            SaveEditModuleCommand.Disable();

            try
            {
                if (!ModuleEditingViewModel.Validate())
                {
                    return;
                }

                ModuleEditingViewModel.CopyTo(ModuleEditingViewModel.SelectedModule);
                SelectedModule = ModuleEditingViewModel.SelectedModule;
                IsModuleEditModeOn = false;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{nameof(SaveEditModuleCommand)}: {e.Message}");
            }
            finally
            {
                SaveEditModuleCommand.Enable();
            }
        }

        #endregion

        #region SaveAddModule command

        public Command SaveAddModuleCommand => _saveAddModuleCommand ??= new Command(ExecuteSaveAddModule, () => true);

        private Command _saveAddModuleCommand;

        private void ExecuteSaveAddModule()
        {
            SaveAddModuleCommand.Disable();

            try
            {
                if (!ModuleEditingViewModel.Validate())
                {
                    return;
                }

                SelectedModule = ModuleEditingViewModel.ToModuleViewModel();
                CustomModules.Add(SelectedModule);
                IsModuleEditModeOn = false;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{nameof(SaveAddModuleCommand)}: {e.Message}");
            }
            finally
            {
                SaveAddModuleCommand.Enable();
            }
        }

        #endregion

        #region CancelEditModule command

        public Command CancelEditModuleCommand => _cancelEditModuleCommand ??=
            new Command(ExecuteCancelEditModule, () => true);

        private Command _cancelEditModuleCommand;

        private void ExecuteCancelEditModule()
        {
            CancelEditModuleCommand.Disable();

            try
            {
                SelectedModule = ModuleEditingViewModel.SelectedModule;
                IsModuleEditModeOn = false;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{nameof(CancelEditModuleCommand)}: {e.Message}");
            }
            finally
            {
                CancelEditModuleCommand.Enable();
            }
        }

        #endregion

        #region AddModule command

        /// <summary>
        ///     Gets the AddModule command.
        /// </summary>
        public Command AddModuleCommand => _addModuleCommand ??= new Command(ExecuteAddModule, CanExecuteAddModule);

        private Command _addModuleCommand;

        /// <summary>
        ///     Method to invoke when the AddModule command is executed.
        /// </summary>
        private void ExecuteAddModule()
        {
            AddModuleCommand.Disable();

            try
            {
                ModuleEditingViewModel.SaveCommand = SaveAddModuleCommand;
                ModuleEditingViewModel.EditingProcessType = ModuleEditingProcessType.Add;
                IsModuleEditModeOn = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{nameof(AddModuleCommand)}: {e.Message}");
            }
            finally
            {
                AddModuleCommand.Enable();
            }
        }

        /// <summary>
        ///     Method to check whether the AddModule command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool CanExecuteAddModule() => true;

        #endregion

        #region EditModule command

        public Command EditModuleCommand => _editModuleCommand ??= new Command(ExecuteEditModule, () => true);

        private Command _editModuleCommand;

        private void ExecuteEditModule()
        {
            EditModuleCommand.Disable();

            try
            {
                SelectedModule.CopyTo(ModuleEditingViewModel);
                ModuleEditingViewModel.SaveCommand = SaveEditModuleCommand;
                ModuleEditingViewModel.SelectedModule = SelectedModule;
                ModuleEditingViewModel.EditingProcessType = ModuleEditingProcessType.Edit;
                IsModuleEditModeOn = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{nameof(EditModuleCommand)}: {e.Message}");
            }
            finally
            {
                EditModuleCommand.Enable();
            }
        }

        #endregion

        #region DeleteModule command

        /// <summary>
        ///     Gets the DeleteModule command.
        /// </summary>
        public Command DeleteModuleCommand =>
            _deleteModuleCommand ??= new Command(ExecuteDeleteModule, CanExecuteDeleteModule);

        private Command _deleteModuleCommand;

        /// <summary>
        ///     Method to invoke when the DeleteModule command is executed.
        /// </summary>
        private void ExecuteDeleteModule()
        {
            DeleteModuleCommand.Disable();

            try
            {
                if (SelectedModule == null)
                {
                    return;
                }

                CustomModules.Remove(SelectedModule);
                SelectedModule = null;
                IsModuleEditModeOn = false;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{nameof(DeleteModuleCommand)}: {e.Message}");
            }
            finally
            {
                DeleteModuleCommand.Enable();
            }
        }

        /// <summary>
        ///     Method to check whether the DeleteModule command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool CanExecuteDeleteModule() => true;

        #endregion
    }
}