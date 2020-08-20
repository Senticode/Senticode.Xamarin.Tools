using _template.Blank.Models;
using Senticode.Xamarin.Tools.MVVM.Abstractions;
using Senticode.Xamarin.Tools.MVVM.Collections;
using Unity;

namespace _template.Blank.ViewModels
{
    internal class MainViewModel : ViewModelBase<AppCommands, AppSettings>
    {
        private readonly ModelController _modelController;

        public MainViewModel()
        {
            Container.RegisterInstance(this);
            _modelController = Container.Resolve<ModelController>();
        }

        public ObservableRangeCollection<WeatherForecastObject> Forecasts => _modelController.Forecasts;

        public string WeatherServiceStatusMessage => "Weather service is offline";
    }
}