using ProjectTemplateWizard.Abstractions;
using ProjectTemplateWizard.Abstractions.Interfaces;

namespace ProjectTemplateWizard.ViewModels
{
    internal class ModuleViewModel : ObservableObjectBase
    {
        #region ModuleType: ModuleType

        /// <summary>
        ///     Gets or sets the ModuleType value.
        /// </summary>
        public ModuleType ModuleType
        {
            get => _moduleType;
            private set => SetProperty(ref _moduleType, value);
        }

        /// <summary>
        ///     ModuleType property data.
        /// </summary>
        private ModuleType _moduleType;

        #endregion

        #region Name: string

        /// <summary>
        ///     Gets or sets the Name value.
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <summary>
        ///     Name property data.
        /// </summary>
        private string _name;

        #endregion

        #region IsXamarinModule: bool

        /// <summary>
        ///     Gets or sets the IsXamarinModule value.
        /// </summary>
        public bool IsXamarinModule
        {
            get => _isXamarinModule;
            set => SetProperty(ref _isXamarinModule, value, OnIsXamarinModuleChanged,
                null, nameof(IsXamarinModule));
        }

        private void OnIsXamarinModuleChanged()
        {
            if (IsXamarinModule)
            {
                ModuleType = ModuleType.Xamarin;
            }
        }

        /// <summary>
        ///     IsXamarinModule property data.
        /// </summary>
        private bool _isXamarinModule;

        #endregion

        #region IsWebModule: bool

        /// <summary>
        ///     Gets or sets the IsWebModule value.
        /// </summary>
        public bool IsWebModule
        {
            get => _isWebModule;
            set => SetProperty(ref _isWebModule, value, OnIsWebModuleChanged, null, nameof(IsWebModule));
        }

        private void OnIsWebModuleChanged()
        {
            if (IsWebModule)
            {
                ModuleType = ModuleType.Web;
            }
        }

        /// <summary>
        ///     IsWebModule property data.
        /// </summary>
        private bool _isWebModule;

        #endregion
    }
}