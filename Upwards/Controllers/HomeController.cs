using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Upwards.Helpers;
using Upwards.Models;

namespace CsvUploader.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private IHostingEnvironment _environment;

        public HomeController(ILogger<HomeController> logger, IHostingEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(IFormFile postedFile)
        {
            if (postedFile != null)
            {
                string path = Path.Combine(this._environment.WebRootPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = Path.GetFileName(postedFile.FileName);
                string filePath = Path.Combine(path, fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                string csvData = System.IO.File.ReadAllText(filePath);
                DataTable dt = new DataTable();
                bool firstRow = true;
                foreach (string row in csvData.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            if (firstRow)
                            {
                                foreach (string cell in row.Split(','))
                                {
                                    dt.Columns.Add(cell.Trim());
                                }
                                dt.Columns.Add("Weather");
                                dt.Columns.Add("Weather Description");
                                firstRow = false;
                            }
                            else
                            {
                                dt.Rows.Add();
                                int i = 0;
                                var cityNm = string.Empty;
                                foreach (string cell in row.Split(','))
                                {
                                    if (i == 0)
                                    {
                                        cityNm = cell;
                                    }

                                    dt.Rows[dt.Rows.Count - 1][i] = cell.Trim();
                                    i++;
                                }

                                //Call Weather API
                                var weather = await GetWeatherByCityAsync(cityNm);
                                string weatherIcnUrl = string.Format(AppSettings.GetValue("WeatherAPIIconUrl"), weather.Weather[0].Icon);
                                dt.Rows[dt.Rows.Count - 1][i] = $"{weather.Main.Temp.ToString().Split('.')[0]}°C;{weatherIcnUrl}";
                                dt.Rows[dt.Rows.Count - 1][i + 1] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(weather.Weather[0].Description.ToLower());
                            }
                        }
                    }
                }

                return View(dt);
            }

            return View();
        }

        public async Task<WeatherData> GetWeatherByCityAsync(string cityNm)
        {
            WeatherData weatherDetails = null;
            var baseUrl = AppSettings.GetValue("UpwardsAPIBaseUrl");

            using (var client = new HttpClient())
            {
                var url = $"{baseUrl}/weather?cityNm={cityNm}";
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsondata = await response.Content.ReadAsStringAsync();
                    var serializedResponse = JsonConvert.DeserializeObject<WeatherData>(jsondata);
                    weatherDetails = serializedResponse;
                }
            }

            return weatherDetails;
        }
    }
}
