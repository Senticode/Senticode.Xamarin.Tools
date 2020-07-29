using Senticode.Xamarin.Tools.Core.Interfaces.Base;
using Template.Mobile.Interfaces.Services.Web;
using Template.WebClientModule.Services;
using Template.WebClientModule.Services.Internal;
using Unity;

namespace Template.WebClientModule
{
    public class ModuleInitializer : IInitializer
    {
        public static ModuleInitializer Instance { get; } = new ModuleInitializer();
        public IUnityContainer Initialize(IUnityContainer container)
        {
            //Internal
            container.RegisterType<IWebClientFactory, WebClientFactory>();
            //External
            container.RegisterType<IWeatherWebService, WeatherWebService>();
            return container;
        }
    }
}
