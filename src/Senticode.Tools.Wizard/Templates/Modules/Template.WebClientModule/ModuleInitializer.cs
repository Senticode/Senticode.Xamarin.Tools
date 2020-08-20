using _template.Mobile.Interfaces.Services.Web;
using _template.WebClientModule.Services;
using _template.WebClientModule.Services.Internal;
using Senticode.Xamarin.Tools.Core.Interfaces.Base;
using Unity;

namespace _template.WebClientModule
{
    public class ModuleInitializer : IInitializer
    {
        public IUnityContainer Initialize(IUnityContainer container)
        {
            //Internal
            container.RegisterType<IWebClientFactory, WebClientFactory>();

            //External
            container.RegisterType<IWeatherWebService, WeatherWebService>();

            return container;
        }

        #region singleton

        private ModuleInitializer()
        {
        }

        public static ModuleInitializer Instance { get; } = new ModuleInitializer();

        #endregion
    }
}