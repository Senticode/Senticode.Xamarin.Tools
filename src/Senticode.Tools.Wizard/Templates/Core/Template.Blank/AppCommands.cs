using Senticode.Xamarin.Tools.Core.Abstractions.Base;
using Unity;

namespace _template.Blank
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

        public override void RegisterTypes(IUnityContainer container)
        {
            //Register your commands here
        }
    }
}