namespace ProjectTemplateWizard.ViewModels
{
    internal class SpecificModuleViewModel : ModuleViewModel
    {
        #region IsApplied: bool

        /// <summary>
        ///     Gets or sets the IsApplied value.
        /// </summary>
        public bool IsApplied
        {
            get => _isApplied;
            set => SetProperty(ref _isApplied, value);
        }

        /// <summary>
        ///     IsApplied property data.
        /// </summary>
        private bool _isApplied;

        #endregion

        #region Description: string

        /// <summary>
        ///     Gets or sets the Description value.
        /// </summary>
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        /// <summary>
        ///     Description property data.
        /// </summary>
        private string _description;

        #endregion
    }
}