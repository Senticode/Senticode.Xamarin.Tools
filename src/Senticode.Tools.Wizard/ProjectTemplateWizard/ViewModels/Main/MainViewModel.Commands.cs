using System;
using System.Diagnostics;
using ProjectTemplateWizard.Core;
using ProjectTemplateWizard.ExtensionMethods;
using ProjectTemplateWizard.Helpers;
using ProjectTemplateWizard.Views;

namespace ProjectTemplateWizard.ViewModels.Main
{
    internal partial class MainViewModel
    {
        #region CloseDialog command

        /// <summary>
        ///     Gets the CloseDialog command.
        /// </summary>
        public Command<ProjectTemplateDialog> CloseDialogCommand =>
            _closeDialogCommand ??= new Command<ProjectTemplateDialog>(ExecuteCloseDialog, CanExecuteCloseDialog);

        private Command<ProjectTemplateDialog> _closeDialogCommand;

        /// <summary>
        ///     Method to invoke when the CloseDialog command is executed.
        /// </summary>
        private void ExecuteCloseDialog(ProjectTemplateDialog dialog)
        {
            CloseDialogCommand.Disable();

            try
            {
                dialog.DialogResult = false;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{nameof(CloseDialogCommand)}: {e.Message}");
            }
            finally
            {
                CloseDialogCommand.Enable();
            }
        }

        /// <summary>
        ///     Method to check whether the CloseDialog command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool CanExecuteCloseDialog(ProjectTemplateDialog parameter) => true;

        #endregion

        #region SubmitDialog command

        /// <summary>
        ///     Gets the SubmitDialog command.
        /// </summary>
        public Command<ProjectTemplateDialog> SubmitDialogCommand =>
            _submitDialogCommand ??= new Command<ProjectTemplateDialog>(ExecuteSubmitDialog, CanExecuteSubmitDialog);

        private Command<ProjectTemplateDialog> _submitDialogCommand;

        /// <summary>
        ///     Method to invoke when the SubmitDialog command is executed.
        /// </summary>
        private void ExecuteSubmitDialog(ProjectTemplateDialog dialog)
        {
            SubmitDialogCommand.Disable();

            try
            {
                if (IsGtkSharpPlatformSelected)
                {
                    GtkSharpReferenceHelper.CheckAvailability();
                }

                dialog.ProjectTemplateData = this.ConvertToProjectTemplateData();
                dialog.DialogResult = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{nameof(SubmitDialogCommand)}: {e.Message}");
            }
            finally
            {
                SubmitDialogCommand.Enable();
            }
        }

        /// <summary>
        ///     Method to check whether the SubmitDialog command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool CanExecuteSubmitDialog(ProjectTemplateDialog parameter) => true;

        #endregion
    }
}