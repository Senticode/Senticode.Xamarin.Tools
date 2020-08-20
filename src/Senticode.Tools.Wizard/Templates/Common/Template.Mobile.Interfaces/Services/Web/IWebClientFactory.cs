using System.Net.Http;

namespace _template.Mobile.Interfaces.Services.Web
{
    public interface IWebClientFactory
    {
        HttpClient GetNewClient();
    }
}