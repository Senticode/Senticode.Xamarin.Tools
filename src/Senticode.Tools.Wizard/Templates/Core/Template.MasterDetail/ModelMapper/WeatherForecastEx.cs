using Template.Common.Entities;
using Template.MasterDetail.Models;

namespace Template.MasterDetail.ModelMapper
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
