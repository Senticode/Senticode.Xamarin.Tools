using Senticode.Xamarin.Tools.Core.Abstractions.Base;
using Template.MasterDetail.Commands.Navigation;

using Unity;

namespace Template.MasterDetail
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
