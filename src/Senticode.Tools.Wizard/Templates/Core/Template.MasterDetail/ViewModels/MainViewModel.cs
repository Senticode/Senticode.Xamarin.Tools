using Senticode.Xamarin.Tools.MVVM.Abstractions;
using Template.MasterDetail.Resources;
using Template.MasterDetail.ViewModels.Menu;
using Unity;

namespace Template.MasterDetail.ViewModels
{
    internal class MainViewModel : ViewModelBase<AppCommands, AppSettings>
    {
        public MainViewModel()
        {
            Container.RegisterInstance(this);
            Title = ResourceKeys.Main;
        }

        public MainMenuViewModel MainMenuViewModel => Container.Resolve<MainMenuViewModel>();

    }
}
