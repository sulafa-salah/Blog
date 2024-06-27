using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Blog.Controllers
{
    [Route("/api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IStringLocalizer<WeatherForecastController> localizer;
        private readonly ILogger<WeatherForecastController> _logger;
        public WeatherForecastController(IStringLocalizer<WeatherForecastController> localizer, ILogger<WeatherForecastController> logger)
        {
            this.localizer = localizer;
            _logger = logger;
        }
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

       
        [HttpGet(Name = "GetWeatherForecast")]
        //[ApiVersion(0.1,Deprecated = true)]
        public IEnumerable<WeatherForecast> GetWeatherForecast()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
           
        }
        [HttpGet("Test")]
        public IActionResult Test() => Ok(new { data = "ss", responseMessage = localizer["SUCCESS"].Value });
    }
}
