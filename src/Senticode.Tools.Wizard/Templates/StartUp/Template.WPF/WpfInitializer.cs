using Senticode.Xamarin.Tools.Core;
using Senticode.Xamarin.Tools.Core.Interfaces.Services;
using Template.WPF.Services;
using Unity;

namespace Template.WPF
{
    internal class WpfInitializer : PlatformInitializerBase
    {
        private WpfInitializer()
        {
        }

        public static WpfInitializer Instance { get; } = new WpfInitializer();

        public override IUnityContainer Initialize(IUnityContainer container)
        {
            container.RegisterType<ILocalize, Localize>();

            return container;
        }
    }
}