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
            set => SetProperty(ref _moduleType, value);
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
    }
}