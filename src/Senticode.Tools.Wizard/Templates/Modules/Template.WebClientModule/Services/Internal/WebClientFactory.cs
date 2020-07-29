using System.Net.Http;
using Template.Mobile.Interfaces.Services.Web;
using Unity;

namespace Template.WebClientModule.Services.Internal
{
    public class WebClientFactory : IWebClientFactory
    {
        private readonly IUnityContainer _container;

        public WebClientFactory(IUnityContainer container)
        {
            container.RegisterInstance(this);
            _container = container;
        }

        public IWebClientSettings WebClientSettings => _container.Resolve<IWebClientSettings>();

        public HttpClient GetNewClient()
        {
            var baseAddress = WebClientSettings.WebServiceAddress;

            var client = new HttpClient(new HttpClientHandler
            {
#if DEBUG
                    ServerCertificateCustomValidationCallback = (a, b, c, d) => true
#endif
            }
            )
            {
                BaseAddress = baseAddress
            };

            return client;
        }
    }
}
