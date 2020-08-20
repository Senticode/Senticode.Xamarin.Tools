using System.Threading.Tasks;
using Senticode.Xamarin.Tools.MVVM.Abstractions;
using Unity;
using Xamarin.Forms;

namespace _template.Blank.ViewModels.Abstractions
{
    /// <summary>
    ///     Dialog view model.
    /// </summary>
    public abstract class DialogViewModelBase : ViewModelBase<AppCommands, AppSettings>
    {
        private Command _goBackCommand;

        private Command _goNextCommand;

        /// <summary>
        ///     Gets the GoBack async command.
        /// </summary>
        public Command GoBackCommand
        {
            get => _goBackCommand ??
                   (_goBackCommand =
                       new Command(async () => await ExecuteGoBackAsync(),
                           CanExecuteGoBack));
            set => _goBackCommand = value;
        }

        /// <summary>
        ///     Gets the GoNext async command.
        /// </summary>
        public Command GoNextCommand => _goNextCommand ??
                                        (_goNextCommand =
                                            new Command(async () => await ExecuteGoNextAsync(),
                                                CanExecuteGoNext));

        /// <summary>
        ///     Method to invoke when the GoBack command is executed.
        /// </summary>
        protected virtual async Task ExecuteGoBackAsync()
        {
            await Container.Resolve<INavigation>().PopModalAsync();
        }

        /// <summary>
        ///     Method to check whether the GoBack command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        protected virtual bool CanExecuteGoBack()
        {
            return true;
        }

        /// <summary>
        ///     Method to invoke when the GoNext command is executed.
        /// </summary>
        protected virtual async Task ExecuteGoNextAsync()
        {
            await Container.Resolve<INavigation>().PopModalAsync();
        }

        /// <summary>
        ///     Method to check whether the GoNext command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        protected virtual bool CanExecuteGoNext()
        {
            return true;
        }
    }
}
