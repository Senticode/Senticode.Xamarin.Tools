using _template.iOS.Services;
using Senticode.Xamarin.Tools.Core;
using Senticode.Xamarin.Tools.Core.Interfaces.Services;
using Unity;

namespace _template.iOS
{
    internal class IosInitializer : PlatformInitializerBase
    {
        public bool IsInitialized { get; set; }

        public override IUnityContainer Initialize(IUnityContainer container)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
            }

            container.RegisterType<ILocalize, Localize>();

            return container;
        }

        #region singleton

        private IosInitializer()
        {
        }

        public static IosInitializer Instance { get; } = new IosInitializer();

        #endregion
    }
}