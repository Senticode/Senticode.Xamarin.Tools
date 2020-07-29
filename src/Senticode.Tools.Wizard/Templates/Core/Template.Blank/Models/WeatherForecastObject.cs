using Senticode.Xamarin.Tools.MVVM.Abstractions;
using System;

namespace Template.Blank.Models
{
    public class WeatherForecastObject : ModelBase
    {
        public Guid Id { get; set; }

        private DateTime _date;
        private int _temperatureC;
        private string _summary;

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
