using _template.Blank.Models;
using _template.Common.Entities;

namespace _template.Blank.ModelMapper
{
    internal static class WeatherForecastEx
    {
        public static WeatherForecastObject MapToModel(this WeatherForecast entity) =>
            new WeatherForecastObject
            {
                Id = entity.Id,
                Date = entity.Date,
                Summary = entity.Summary,
                TemperatureC = entity.TemperatureC
            };
    }
}