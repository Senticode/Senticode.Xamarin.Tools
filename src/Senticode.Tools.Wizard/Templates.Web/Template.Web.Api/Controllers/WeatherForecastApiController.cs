using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Senticode.Base;
using Template.Common.Entities;

namespace Template.Web.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastApiController : ControllerBase
    {
        private static readonly string[] Summaries =
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastApiController> _logger;

        public WeatherForecastApiController(ILogger<WeatherForecastApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public Result<IEnumerable<WeatherForecast>> Get()
        {
            var rng = new Random();
            var array = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToList();
            return new Result<IEnumerable<WeatherForecast>>(array);
        }
    }
}