using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Senticode.Xamarin.Tools.Core.Interfaces.Services;
using Senticode.Xamarin.Tools.MVVM.Abstractions;
using Senticode.Xamarin.Tools.MVVM.Collections;
using Template.MasterDetail.Models;
using Template.MasterDetail.Resources;
using Unity;
using Xamarin.Forms;

namespace Template.MasterDetail.ViewModels.Menu
{
    internal class LanguageMenuViewModel : ViewModelBase<AppCommands, AppSettings>
    {
        private readonly ILocalize _localize;

        public LanguageMenuViewModel(IUnityContainer container, ILocalize localize)
        {
            _localize = localize;
            Languages.ReplaceAll(localize.AvailableImplementation.Select(x => new LanguageObject(x)).ToList());
            container.RegisterInstance(this);
            Title = ResourceKeys.Languages;
            ExecuteSetLanguage(_localize.CultureContext);
        }

        public ObservableRangeCollection<LanguageObject> Languages { get; } =
            new ObservableRangeCollection<LanguageObject>();

        #region SetLanguageCommand

        /// <summary>
        ///     Gets the SetLanguage command.
        /// </summary>
        public Command<CultureInfo> SetLanguageCommand => _setLanguageCommand ??
                                                          (_setLanguageCommand =
                                                              new Command<CultureInfo>(ExecuteSetLanguage,
                                                                  CanExecuteSetLanguage));

        private Command<CultureInfo> _setLanguageCommand;


        /// <summary>
        ///     Method to invoke when the SetLanguage command is executed.
        /// </summary>
        private void ExecuteSetLanguage(CultureInfo parameter)
        {
            if (!Equals(_localize.CultureContext, parameter))
            {
                _localize.CultureContext = parameter;
                AppSettings.Language = parameter.TwoLetterISOLanguageName;
                Task.Run(async () => await AppSettings.SaveAsync());
            }

            foreach (var language in Languages)
            {
                language.IsChecked = Equals(language.Culture, parameter);
            }
        }

        /// <summary>
        ///     Method to check whether the SetLanguage command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool CanExecuteSetLanguage(CultureInfo parameter)
        {
            return true;
        }

        #endregion
    }
}