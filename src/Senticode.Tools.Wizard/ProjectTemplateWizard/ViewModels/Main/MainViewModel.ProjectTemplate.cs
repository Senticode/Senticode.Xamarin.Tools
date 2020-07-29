using ProjectTemplateWizard.Abstractions.Interfaces;

namespace ProjectTemplateWizard.ViewModels.Main
{
    internal partial class MainViewModel
    {
        #region ProjectTemplateType: ProjectTemplateType

        /// <summary>
        ///     Gets or sets the ProjectTemplateType value.
        /// </summary>
        public ProjectTemplateType ProjectTemplateType
        {
            get => _projectTemplateType;
            set => SetProperty(ref _projectTemplateType, value);
        }

        /// <summary>
        ///     ProjectTemplateType property data.
        /// </summary>
        private ProjectTemplateType _projectTemplateType;

        #endregion

        #region IsMasterDetailProjectSelected: bool

        /// <summary>
        ///     Gets or sets the IsMasterDetailProjectSelected value.
        /// </summary>
        public bool IsMasterDetailProjectSelected
        {
            get => _isMasterDetailProjectSelected;
            set => SetProperty(ref _isMasterDetailProjectSelected, value,
                OnIsMasterDetailProjectSelectedChanged, null, nameof(IsMasterDetailProjectSelected));
        }

        private void OnIsMasterDetailProjectSelectedChanged()
        {
            if (IsMasterDetailProjectSelected)
            {
                ProjectTemplateType = ProjectTemplateType.MasterDetail;
            }
        }

        /// <summary>
        ///     IsMasterDetailProjectSelected property data.
        /// </summary>
        private bool _isMasterDetailProjectSelected;

        #endregion

        #region IsBlankProjectSelected: bool

        /// <summary>
        ///     Gets or sets the IsBlankProjectSelected value.
        /// </summary>
        public bool IsBlankProjectSelected
        {
            get => _isBlankProjectSelected;
            set => SetProperty(ref _isBlankProjectSelected, value,
                OnIsBlankProjectSelectedChanged, null, nameof(IsBlankProjectSelected));
        }

        private void OnIsBlankProjectSelectedChanged()
        {
            if (IsBlankProjectSelected)
            {
                ProjectTemplateType = ProjectTemplateType.Blank;
            }
        }

        /// <summary>
        ///     IsBlankProjectSelected property data.
        /// </summary>
        private bool _isBlankProjectSelected;

        #endregion

        #region IsShellProjectSelected: bool

        /// <summary>
        ///     Gets or sets the IsShellProjectSelected value.
        /// </summary>
        public bool IsShellProjectSelected
        {
            get => _isShellProjectSelected;
            set => SetProperty(ref _isShellProjectSelected, value,
                OnIsShellProjectSelectedChanged, null, nameof(IsShellProjectSelected));
        }

        private void OnIsShellProjectSelectedChanged()
        {
            if (IsShellProjectSelected)
            {
                ProjectTemplateType = ProjectTemplateType.Shell;
            }
        }

        /// <summary>
        ///     IsShellProjectSelected property data.
        /// </summary>
        private bool _isShellProjectSelected;

        #endregion
    }
}