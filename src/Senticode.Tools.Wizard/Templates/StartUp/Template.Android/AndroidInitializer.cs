using Senticode.Xamarin.Tools.Core;
using Senticode.Xamarin.Tools.Core.Interfaces.Services;
using Template.Android.Services;
using Unity;

namespace Template.Android
{
    public class AndroidInitializer : PlatformInitializerBase
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

        private AndroidInitializer()
        {
        }

        public static AndroidInitializer Instance { get; } = new AndroidInitializer();

        #endregion
    }
}