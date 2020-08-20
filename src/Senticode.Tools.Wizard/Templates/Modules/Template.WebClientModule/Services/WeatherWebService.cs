using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using _template.Common.Entities;
using _template.Common.Web.Infrastructure;
using _template.Common.Web.Infrastructure.Extensions;
using _template.Mobile.Interfaces.Services.Web;
using _template.WebClientModule.Services.Internal;
using Senticode.Base;
using Senticode.Base.Interfaces;
using Senticode.Base.Services;
using Unity;

namespace _template.WebClientModule.Services
{
    internal class WeatherWebService : ServiceBase, IWeatherWebService
    {
        private readonly IUnityContainer _container;
        private readonly IWebClientSettings _settings;

        public WeatherWebService(IUnityContainer container, IWebClientSettings settings)
        {
            _settings = settings;
            _container = container.RegisterInstance<IWeatherWebService>(this);
        }

        public async Task<IResult<IEnumerable<WeatherForecast>>> GetAllAsync()
        {
            var request = WebApi.WeatherForecastApiController.GetAll();

            using (var client = _container.Resolve<WebClientFactory>().GetNewClient())
            {
                HttpResponseMessage response;
                try
                {
                    response = await client.GetAsync(request);
                }
                catch (Exception e)
                {
                    return new Result<IEnumerable<WeatherForecast>>(e);
                }

                if (response.IsSuccessStatusCode)
                {
                    var forecasts = await response.Content.ReadAsStringAsync();
                    var result = forecasts.ToObjectFromJsonString<List<WeatherForecast>>();
                    return new Result<IEnumerable<WeatherForecast>>(result);
                }

                return new Result<IEnumerable<WeatherForecast>>(new Exception(response.ReasonPhrase));
            }
        }
    }
}