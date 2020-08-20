namespace _template.Common.Web.Infrastructure.API
{
    public class WeatherForecastApi
    {
        public string GetAll() => $"/{nameof(WeatherForecastApi)}";
    }
}