using System.Net.Http;

namespace Template.Mobile.Interfaces.Services.Web
{
    public interface IWebClientFactory
    {
        HttpClient GetNewClient();
    }
}