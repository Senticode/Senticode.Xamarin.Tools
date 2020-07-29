using Senticode.Xamarin.Tools.Core;
using Senticode.Xamarin.Tools.Core.Interfaces.Services;
using Template.GtkSharp.Services;
using Unity;

namespace Template.GtkSharp
{
    internal class GtkSharpInitializer : PlatformInitializerBase
    {
        private GtkSharpInitializer()
        {
        }

        public static GtkSharpInitializer Instance { get; } = new GtkSharpInitializer();

        public override IUnityContainer Initialize(IUnityContainer container)
        {
            container.RegisterType<ILocalize, Localize>();

            return container;
        }
    }
}