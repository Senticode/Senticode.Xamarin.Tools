﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _template.Blank.ModelMapper;
using _template.Blank.Models;
using _template.Common.Entities;
using Senticode.Xamarin.Tools.MVVM.Collections;
using Unity;
using Xamarin.Forms;

namespace _template.Blank
{
    internal class ModelController
    {
        private IUnityContainer _container;

        public ModelController(IUnityContainer container)
        {
            _container = container.RegisterInstance(this);
        }

        public ObservableRangeCollection<WeatherForecastObject> Forecasts { get; } =
            new ObservableRangeCollection<WeatherForecastObject>();

        public async Task ReplaceAllObjectsAsync<TModel>(IEnumerable<TModel> entities)
        {
            await Task.Run(() =>
            {
                if (entities is IEnumerable<WeatherForecast> items)
                {
                    var models = items.Select(x => x.MapToModel()).ToList();
                    Device.BeginInvokeOnMainThread(() => Forecasts.ReplaceAll(models));
                }
            });
        }
    }
}