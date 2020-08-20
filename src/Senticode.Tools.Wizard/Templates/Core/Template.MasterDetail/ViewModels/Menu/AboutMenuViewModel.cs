using _template.MasterDetail.Resources;
using Senticode.Xamarin.Tools.MVVM.Abstractions;
using Unity;

namespace _template.MasterDetail.ViewModels.Menu
{
    internal class AboutMenuViewModel : ViewModelBase<AppCommands, AppSettings>
    {
        public AboutMenuViewModel(IUnityContainer container)
        {
            container.RegisterInstance(this);

            Title = ResourceKeys.About;
        }
    }
}