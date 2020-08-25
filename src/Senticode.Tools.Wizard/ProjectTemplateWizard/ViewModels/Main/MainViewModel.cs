using ProjectTemplateWizard.Abstractions;
using ProjectTemplateWizard.Abstractions.Interfaces;
using ProjectTemplateWizard.ExtensionMethods;

namespace ProjectTemplateWizard.ViewModels.Main
{
    internal partial class MainViewModel : ObservableObjectBase
    {
        public MainViewModel()
        {
            InitializeModules();
            InitializeAssetPickers();
            InitializeModuleEditing();
            IsMasterDetailProjectSelected = true;
            WebDatabaseInfrastructureType = WebDatabaseInfrastructureType.MsSql;
            XamarinDatabaseInfrastructureType = XamarinDatabaseInfrastructureType.SqLite;
        }

        #region IsModularDesign: bool

        /// <summary>
        ///     Gets or sets the IsModularDesign value.
        /// </summary>
        public bool IsModularDesign
        {
            get => _isModularDesign;
            set => SetProperty(ref _isModularDesign, value,
                this.UpdateArchitecturePatterSelection, null, nameof(IsModularDesign));
        }

        /// <summary>
        ///     IsModularDesign property data.
        /// </summary>
        private bool _isModularDesign;

        #endregion

        #region IsReadmeIncluded: bool

        /// <summary>
        ///     Gets or sets the IsReadmeIncluded value.
        /// </summary>
        public bool IsReadmeIncluded
        {
            get => _isReadmeIncluded;
            set => SetProperty(ref _isReadmeIncluded, value);
        }

        /// <summary>
        ///     IsReadmeIncluded property data.
        /// </summary>
        private bool _isReadmeIncluded;

        #endregion

        #region IsVersioningSystemIncluded: bool

        /// <summary>
        ///     Gets or sets the IsVersioningSystemIncluded value.
        /// </summary>
        public bool IsVersioningSystemIncluded
        {
            get => _isVersioningSystemIncluded;
            set => SetProperty(ref _isVersioningSystemIncluded, value);
        }

        /// <summary>
        ///     IsVersioningSystemIncluded property data.
        /// </summary>
        private bool _isVersioningSystemIncluded;

        #endregion

        #region IsLicensesInfoPageIncluded: bool

        /// <summary>
        ///     Gets or sets the IsLicensesInfoPageIncluded value.
        /// </summary>
        public bool IsLicensesInfoPageIncluded
        {
            get => _isLicensesInfoPageIncluded;
            set => SetProperty(ref _isLicensesInfoPageIncluded, value);
        }

        /// <summary>
        ///     IsLicensesInfoPageIncluded property data.
        /// </summary>
        private bool _isLicensesInfoPageIncluded;

        #endregion

        #region IsNUnitIncluded: bool

        /// <summary>
        ///     Gets or sets the IsNUnitIncluded value.
        /// </summary>
        public bool IsNUnitIncluded
        {
            get => _isNUnitIncluded;
            set => SetProperty(ref _isNUnitIncluded, value);
        }

        /// <summary>
        ///     IsNUnitIncluded property data.
        /// </summary>
        private bool _isNUnitIncluded;

        #endregion

        #region XamarinDatabaseInfrastructureType: XamarinDatabaseInfrastructureType

        /// <summary>
        ///     Gets or sets the XamarinDatabaseInfrastructureType value.
        /// </summary>
        public XamarinDatabaseInfrastructureType XamarinDatabaseInfrastructureType
        {
            get => _xamarinDatabaseInfrastructureType;
            set => SetProperty(ref _xamarinDatabaseInfrastructureType, value,
                OnXamarinDatabaseInfrastructureTypeChanged, null, nameof(XamarinDatabaseInfrastructureType));
        }

        private void OnXamarinDatabaseInfrastructureTypeChanged()
        {
            DatabaseXamarinModule.Description = XamarinDatabaseInfrastructureType.GetDisplayName();
        }

        /// <summary>
        ///     XamarinDatabaseInfrastructureType property data.
        /// </summary>
        private XamarinDatabaseInfrastructureType _xamarinDatabaseInfrastructureType;

        #endregion

        #region WebDatabaseInfrastructureType: WebDatabaseInfrastructureType

        /// <summary>
        ///     Gets or sets the WebDatabaseInfrastructureType value.
        /// </summary>
        public WebDatabaseInfrastructureType WebDatabaseInfrastructureType
        {
            get => _webDatabaseInfrastructureType;
            set => SetProperty(ref _webDatabaseInfrastructureType, value,
                OnWebDatabaseInfrastructureTypeChanged, null, nameof(WebDatabaseInfrastructureType));
        }

        private void OnWebDatabaseInfrastructureTypeChanged()
        {
            DatabaseWebModule.Description = WebDatabaseInfrastructureType.GetDisplayName();
        }

        /// <summary>
        ///     WebDatabaseInfrastructureType property data.
        /// </summary>
        private WebDatabaseInfrastructureType _webDatabaseInfrastructureType;

        #endregion

        #region IsWebBackendIncluded: bool

        /// <summary>
        ///     Gets or sets the IsWebBackendIncluded value.
        /// </summary>
        public bool IsWebBackendIncluded
        {
            get => _isWebBackendIncluded;
            set => SetProperty(ref _isWebBackendIncluded, value,
                this.UpdateWebBackendSelection, null, nameof(IsWebBackendIncluded));
        }

        /// <summary>
        ///     IsWebBackendIncluded property data.
        /// </summary>
        private bool _isWebBackendIncluded;

        #endregion

        #region IsDockerComposeIncluded: bool

        /// <summary>
        ///     Gets or sets the IsDockerComposeIncluded value.
        /// </summary>
        public bool IsDockerComposeIncluded
        {
            get => _isDockerComposeIncluded;
            set => SetProperty(ref _isDockerComposeIncluded, value);
        }

        /// <summary>
        ///     IsDockerComposeIncluded property data.
        /// </summary>
        private bool _isDockerComposeIncluded;

        #endregion

        #region IsSwaggerIncluded: bool

        /// <summary>
        ///     Gets or sets the IsSwaggerIncluded value.
        /// </summary>
        public bool IsSwaggerIncluded
        {
            get => _isSwaggerIncluded;
            set => SetProperty(ref _isSwaggerIncluded, value);
        }

        /// <summary>
        ///     IsSwaggerIncluded property data.
        /// </summary>
        private bool _isSwaggerIncluded;

        #endregion

        #region IsIdentityServerIncluded: bool

        /// <summary>
        ///     Gets or sets the IsIdentityServerIncluded value.
        /// </summary>
        public bool IsIdentityServerIncluded
        {
            get => _isIdentityServerIncluded;
            set => SetProperty(ref _isIdentityServerIncluded, value);
        }

        /// <summary>
        ///     IsIdentityServerIncluded property data.
        /// </summary>
        private bool _isIdentityServerIncluded;

        #endregion

        #region IsAndroidPlatformSelected: bool

        /// <summary>
        ///     Gets or sets the IsAndroidPlatformSelected value.
        /// </summary>
        public bool IsAndroidPlatformSelected
        {
            get => _isAndroidPlatformSelected;
            set => SetProperty(ref _isAndroidPlatformSelected, value,
                this.UpdatePlatformSelection, null, nameof(IsAndroidPlatformSelected));
        }

        /// <summary>
        ///     IsAndroidPlatformSelected property data.
        /// </summary>
        private bool _isAndroidPlatformSelected;

        #endregion

        #region IsIosPlatformSelected: bool

        /// <summary>
        ///     Gets or sets the IsIosPlatformSelected value.
        /// </summary>
        public bool IsIosPlatformSelected
        {
            get => _isIosPlatformSelected;
            set => SetProperty(ref _isIosPlatformSelected, value,
                this.UpdatePlatformSelection, null, nameof(IsIosPlatformSelected));
        }

        /// <summary>
        ///     IsIosPlatformSelected property data.
        /// </summary>
        private bool _isIosPlatformSelected;

        #endregion

        #region IsUwpPlatformSelected: bool

        /// <summary>
        ///     Gets or sets the IsUwpPlatformSelected value.
        /// </summary>
        public bool IsUwpPlatformSelected
        {
            get => _isUwpPlatformSelected;
            set => SetProperty(ref _isUwpPlatformSelected, value,
                this.UpdatePlatformSelection, null, nameof(IsUwpPlatformSelected));
        }

        /// <summary>
        ///     IsUwpPlatformSelected property data.
        /// </summary>
        private bool _isUwpPlatformSelected;

        #endregion

        #region IsWpfPlatformSelected: bool

        /// <summary>
        ///     Gets or sets the IsWpfPlatformSelected value.
        /// </summary>
        public bool IsWpfPlatformSelected
        {
            get => _isWpfPlatformSelected;
            set => SetProperty(ref _isWpfPlatformSelected, value,
                this.UpdatePlatformSelection, null, nameof(IsWpfPlatformSelected));
        }

        /// <summary>
        ///     IsWpfPlatformSelected property data.
        /// </summary>
        private bool _isWpfPlatformSelected;

        #endregion

        #region IsGtkSharpPlatformSelected: bool

        /// <summary>
        ///     Gets or sets the IsGtkSharpPlatformSelected value.
        /// </summary>
        public bool IsGtkSharpPlatformSelected
        {
            get => _isGtkSharpPlatformSelected;
            set => SetProperty(ref _isGtkSharpPlatformSelected, value,
                this.UpdatePlatformSelection, null, nameof(IsGtkSharpPlatformSelected));
        }

        /// <summary>
        ///     IsGtkSharpPlatformSelected property data.
        /// </summary>
        private bool _isGtkSharpPlatformSelected;

        #endregion

        #region IsAnyPlatformSelected: bool

        /// <summary>
        ///     Gets or sets the IsAnyPlatformSelected value.
        /// </summary>
        public bool IsAnyPlatformSelected
        {
            get => _isAnyPlatformSelected;
            set => SetProperty(ref _isAnyPlatformSelected, value);
        }

        /// <summary>
        ///     IsAnyPlatformSelected property data.
        /// </summary>
        private bool _isAnyPlatformSelected;

        #endregion
    }
}