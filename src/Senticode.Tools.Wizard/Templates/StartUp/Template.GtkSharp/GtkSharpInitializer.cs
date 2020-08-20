using _template.GtkSharp.Services;
using Senticode.Xamarin.Tools.Core;
using Senticode.Xamarin.Tools.Core.Interfaces.Services;
using Unity;

namespace _template.GtkSharp
{
    internal class GtkSharpInitializer : PlatformInitializerBase
    {
        public override IUnityContainer Initialize(IUnityContainer container)
        {
            container.RegisterType<ILocalize, Localize>();

            return container;
        }

        #region singleton

        private GtkSharpInitializer()
        {
        }

        public static GtkSharpInitializer Instance { get; } = new GtkSharpInitializer();

        #endregion
    }
}