using System.Collections.Generic;
using System.Threading.Tasks;
using Senticode.Base.Interfaces;
using Template.Common.Entities;

namespace Template.Mobile.Interfaces.Services.Web
{
    public interface IWeatherWebService
    {
        Task<IResult<IEnumerable<WeatherForecast>>> GetAllAsync();
    }
}