using _template.Common.Web.Infrastructure.API;

namespace _template.Common.Web.Infrastructure
{
    public static class WebApi
    {
        public static WeatherForecastApi WeatherForecastApiController { get; } = new WeatherForecastApi();
    }
}