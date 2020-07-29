using Template.Blank.Models;
using Template.Common.Entities;

namespace Template.Blank.ModelMapper
{
    internal static class WeatherForecastEx
    {
        public static WeatherForecastObject MapToModel(this WeatherForecast entity)
        {
            return new WeatherForecastObject()
            {
                Id = entity.Id,
                Date = entity.Date,
                Summary = entity.Summary,
                TemperatureC = entity.TemperatureC
            };
        }
    }
}
