using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Upwords.API.Services.Interface;

namespace Upwords.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;
        private readonly IWeatherService _weatherService;

        public WeatherController(ILogger<WeatherController> logger, IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(string cityNm)
        {
            _logger.LogInformation("Get weather started.");
            return Ok(await _weatherService.GetWeatherByCityAsync(cityNm));
        }
    }
}
