using Senticode.Xamarin.Tools.MVVM.Abstractions;
using Unity;

namespace _template.MasterDetail.ViewModels
{
    public class SettingsViewModel : ViewModelBase<AppCommands, AppSettings>
    {
        public SettingsViewModel(IUnityContainer container)
        {
            container.RegisterInstance(this);
        }
    }
}