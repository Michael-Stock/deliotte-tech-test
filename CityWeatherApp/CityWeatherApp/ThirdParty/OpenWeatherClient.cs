using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CityWeatherApp.ThirdParty
{
    public class OpenWeatherClient : IOpenWeatherClient
    {
        private IHttpClientFactory clientFactory;
        private string apiKey = "c55cbb2d4bdf447e63a8c57f3e434adb";

        public OpenWeatherClient(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public async Task<CityWeatherResponse> GetCityWeather(string name)
        {
            List <GeoCoordinatesResponse> coordinatesResponse = await GetGeoCoordinates(name);

            if (coordinatesResponse == null)
            {
                return null;
            }

            GeoCoordinatesResponse cityCoordinates = coordinatesResponse.First();

            CityWeatherResponse result = await GetWeather(cityCoordinates);

            return result;
        }
        private async Task<List<GeoCoordinatesResponse>> GetGeoCoordinates(string name)
        {
            HttpClient client = clientFactory.CreateClient();

            string requestUri = $"http://api.openweathermap.org/geo/1.0/direct?q={name}&appid={apiKey}";

            HttpResponseMessage response = await client.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            using var responseStream = await response.Content.ReadAsStreamAsync();
            List<GeoCoordinatesResponse> result = await JsonSerializer.DeserializeAsync<List<GeoCoordinatesResponse>>(responseStream);

            return result;
        }

        private async Task<CityWeatherResponse> GetWeather(GeoCoordinatesResponse cityCoordinates)
        {
            HttpClient client = clientFactory.CreateClient();

            string requestUri = $"https://api.openweathermap.org/data/2.5/weather?lat={cityCoordinates.lat}&lon={cityCoordinates.lon}&appid={apiKey}";

            HttpResponseMessage response = await client.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            using var responseStream = await response.Content.ReadAsStreamAsync();
            CityWeatherResponse result = await JsonSerializer.DeserializeAsync<CityWeatherResponse>(responseStream);

            return result;
        }
    }

    public interface IOpenWeatherClient
    {
        public Task<CityWeatherResponse> GetCityWeather(string name);
    }

    public class GeoCoordinatesResponse
    {
        public decimal lat { get; set; }
        public decimal lon { get; set; }
    }

    public class CityWeatherResponse
    {
        public List<CityWeatherEntry> weather { get; set; }
    }

    public class CityWeatherEntry
    {
        public string main { get; set; }
    }
}
