using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Upwords.API.Helpers;
using Upwords.API.Models;
using Upwords.API.Services.Interface;

namespace Upwords.API.Services
{
    public class WeatherService : IWeatherService
    {
        public async Task<WeatherData> GetWeatherByCityAsync(string cityNm)
        {
            try
            {
                WeatherData weatherDetails = null;
                var apiKey = AppSettings.GetValue("WeatherAPIKey");
                var baseUrl = AppSettings.GetValue("WeatherAPIBaseUrl");

                using (var client = new HttpClient())
                {
                    var url = string.Format(baseUrl, cityNm, apiKey);
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var rootresponse = JsonConvert.DeserializeObject<WeatherData>(jsonString);
                        weatherDetails = rootresponse;
                    }
                }

                return weatherDetails;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
