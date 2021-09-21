using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;
using Upwords.API.Services;
using Upwords.API.Services.Interface;

namespace Upwards.API.Tests
{
    [TestClass]
    public class WeatherServiceTests
    {
        IWeatherService _weatherService;

        [TestInitialize]
        public void Initialize()
        {
            _weatherService = new WeatherService();
        }

        [TestMethod]
        public async Task GetWeatherByCityAsync()
        {
            var city = "Manila";
            var result = await _weatherService.GetWeatherByCityAsync(city);
            Assert.IsNotNull(result);
        }
    }
}
