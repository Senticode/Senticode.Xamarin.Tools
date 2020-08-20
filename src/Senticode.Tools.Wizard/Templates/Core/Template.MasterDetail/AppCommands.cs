using _template.MasterDetail.Commands.Navigation;
using Senticode.Xamarin.Tools.Core.Abstractions.Base;
using Unity;

namespace _template.MasterDetail
{
    public class AppCommands : AppCommandsBase<AppSettings>
    {
        public AppCommands(IUnityContainer container) : base(container)
        {
            if (!container.IsRegistered<AppCommands>())
            {
                container.RegisterInstance(this);
            }
        }

        public NavigateToPageCommand NavigateToPageCommand => Container.Resolve<NavigateToPageCommand>();

        public NavigateToMenuCommand NavigateToMenuCommand => Container.Resolve<NavigateToMenuCommand>();

        public override void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<NavigateToPageCommand>()
                .RegisterType<NavigateToMenuCommand>()
                ;
        }
    }
}