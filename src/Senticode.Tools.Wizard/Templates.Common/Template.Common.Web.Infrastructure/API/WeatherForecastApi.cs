namespace Template.Common.Web.Infrastructure.API
{
    public class WeatherForecastApi
    {
        public string GetAll()
        {
            return $"/{nameof(WeatherForecastApi)}";
        }
    }
}
