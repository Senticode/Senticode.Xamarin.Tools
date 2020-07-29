using Template.Common.Web.Infrastructure.API;

namespace Template.Common.Web.Infrastructure
{
    public static class WebApi
    {
        public static WeatherForecastApi WeatherForecastApiController { get; } = new WeatherForecastApi();
    }
}
