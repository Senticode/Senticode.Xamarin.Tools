using Senticode.Xamarin.Tools.Core;
using Senticode.Xamarin.Tools.Core.Interfaces.Services;
using Template.UWP.Services;
using Unity;

namespace Template.UWP
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