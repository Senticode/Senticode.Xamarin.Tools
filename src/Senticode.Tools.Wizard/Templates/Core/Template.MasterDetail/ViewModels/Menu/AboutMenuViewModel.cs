using _template.MasterDetail.Resources;
using Senticode.Xamarin.Tools.MVVM.Abstractions;
using Unity;

namespace _template.MasterDetail.ViewModels.Menu
{
    internal sealed class AboutMenuViewModel : ViewModelBase<AppCommands, AppSettings>
    {
        public AboutMenuViewModel(IUnityContainer container)
        {
            container.RegisterInstance(this);

            /*_licensesInfo_*/
            /*LicenseInfo =
                new ActionViewModel(ResourceKeys.Licenses)
                {
                    Command = Container.Resolve<NavigateToMenuCommand>(),
                    Parameter = MenuKind.Licenses
                };*/

            Title = ResourceKeys.About;
        }

        /*_licensesInfoProperty_*/ /*public ActionViewModel LicenseInfo { get; }*/
    }
}