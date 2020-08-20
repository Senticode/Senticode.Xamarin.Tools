using System;
using Senticode.Base;

namespace _template.Common.Entities
{
    public class WeatherForecast : Entity<Guid>
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public string Summary { get; set; }
    }
}