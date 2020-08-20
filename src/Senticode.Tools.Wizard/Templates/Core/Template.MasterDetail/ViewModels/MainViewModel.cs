using _template.MasterDetail.Resources;
using _template.MasterDetail.ViewModels.Menu;
using Senticode.Xamarin.Tools.MVVM.Abstractions;
using Unity;

namespace _template.MasterDetail.ViewModels
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