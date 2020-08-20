using System.Collections.Generic;
using System.Threading.Tasks;
using _template.Common.Entities;
using Senticode.Base.Interfaces;

namespace _template.Mobile.Interfaces.Services.Web
{
    public interface IWeatherWebService
    {
        Task<IResult<IEnumerable<WeatherForecast>>> GetAllAsync();
    }
}