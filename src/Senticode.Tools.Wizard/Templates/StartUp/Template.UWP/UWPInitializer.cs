using _template.UWP.Services;
using Senticode.Xamarin.Tools.Core;
using Senticode.Xamarin.Tools.Core.Interfaces.Services;
using Unity;

namespace _template.UWP
{
    public class UwpInitializer : PlatformInitializerBase
    {
        public override IUnityContainer Initialize(IUnityContainer container)
        {
            container.RegisterType<ILocalize, Localize>();

            return container;
        }

        #region singleton

        private UwpInitializer()
        {
        }

        public static UwpInitializer Instance { get; } = new UwpInitializer();

        #endregion
    }
}