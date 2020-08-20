using _template.Common.Entities;
using _template.MasterDetail.Models;

namespace _template.MasterDetail.ModelMapper
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