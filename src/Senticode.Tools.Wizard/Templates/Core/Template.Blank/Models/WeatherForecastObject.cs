using System;
using Senticode.Xamarin.Tools.MVVM.Abstractions;

namespace _template.Blank.Models
{
    public class WeatherForecastObject : ModelBase
    {
        private DateTime _date;
        private string _summary;
        private int _temperatureC;
        public Guid Id { get; set; }

        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        public int TemperatureC
        {
            get => _temperatureC;
            set => SetProperty(ref _temperatureC, value);
        }

        public string Summary
        {
            get => _summary;
            set => SetProperty(ref _summary, value);
        }
    }
}