using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace WeatherApp.Controllers
{
    public class WeatherController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "X"; // Your OpenWeather API key

        public WeatherController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Vis vejret for en by
        public async Task<IActionResult> Index(string city = "Aarhus")
        {
            // URL API
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric$lang=da";

            // send request til OpenWeather API
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            string responseData = await response.Content.ReadAsStringAsync();

            // fejlhåndtering med fejlmeddelse
            if (!response.IsSuccessStatusCode)
            {
                ViewData["Error"] = "Unable to fetch weather data. Please check your API key or try again later.";
                return View();
            }

            // deserialize JSON svaret til et objekt (JsonElement)
            var weatherData = JsonSerializer.Deserialize<JsonElement>(responseData);

            // udtræk specifik vejr data fra objektet
            var temperature = weatherData.GetProperty("main").GetProperty("temp").GetDecimal();
            var humidity = weatherData.GetProperty("main").GetProperty("humidity").GetInt32();
            var description = weatherData.GetProperty("weather")[0].GetProperty("description").GetString();
            var icon = weatherData.GetProperty("weather")[0].GetProperty("icon").GetString();   // Opgave 3

            // Opgave 3: URL til ikonet
            string iconUrl = $"https://openweathermap.org/img/wn/{icon}.png";

            // passer data til viewet
            ViewData["City"] = city;
            ViewData["Temperature"] = temperature;
            ViewData["Humidity"] = humidity;
            ViewData["Description"] = description;
            ViewData["IconUrl"] = iconUrl; // Opgave 3

            return View();
        }
    }
}
