using _template.MasterDetail.Models;
using Senticode.Xamarin.Tools.MVVM.Abstractions;
using Senticode.Xamarin.Tools.MVVM.Collections;
using Unity;

namespace _template.MasterDetail.ViewModels
{
    public class HomeViewModel : ViewModelBase<AppCommands, AppSettings>
    {
        private readonly ModelController _modelController;

        public HomeViewModel(IUnityContainer container)
        {
            container.RegisterInstance(this);
            _modelController = Container.Resolve<ModelController>();
        }

        public ObservableRangeCollection<WeatherForecastObject> Forecasts => _modelController.Forecasts;

        public string WeatherServiceStatusMessage => "Weather service is offline";
    }
}