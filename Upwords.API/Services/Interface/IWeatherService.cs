using System.Threading.Tasks;
using Upwords.API.Models;

namespace Upwords.API.Services.Interface
{
    public interface IWeatherService
    {
        Task<WeatherData> GetWeatherByCityAsync(string cityNm);
    }
}
