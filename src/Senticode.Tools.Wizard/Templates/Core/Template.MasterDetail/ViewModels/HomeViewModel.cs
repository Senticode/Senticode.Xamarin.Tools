using System.Linq;
using Senticode.Xamarin.Tools.MVVM.Abstractions;
using Senticode.Xamarin.Tools.MVVM.Collections;
using Template.MasterDetail.Models;
using Unity;

namespace Template.MasterDetail.ViewModels
{
    public class HomeViewModel : ViewModelBase<AppCommands, AppSettings>
    {
        private ModelController _modelController;
        public HomeViewModel(IUnityContainer container)
        {
            container.RegisterInstance(this);
            _modelController = Container.Resolve<ModelController>();
        }

        public ObservableRangeCollection<WeatherForecastObject> Forecasts => _modelController.Forecasts;

        public string WeatherServiceStatusMessage => "Weather service is offline";
    }
}