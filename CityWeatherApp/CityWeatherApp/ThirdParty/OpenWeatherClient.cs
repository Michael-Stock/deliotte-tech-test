using CityWeatherApp.Configuration;
using Microsoft.Extensions.Options;
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
        private string _weatherApiUrl;
        private string _geoApiUrl;
        private string _apiKey;

        public OpenWeatherClient(IHttpClientFactory clientFactory, IOptions<ExternalApiOptions> configuration)
        {
            this.clientFactory = clientFactory;
            this._weatherApiUrl = configuration.Value.WeatherApiUrl;
            this._geoApiUrl = configuration.Value.GeoApiUrl;
            this._apiKey = configuration.Value.WeatherApiKey;
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
            using HttpClient client = clientFactory.CreateClient();

            string requestUri = $"{_geoApiUrl}/geo/1.0/direct?q={name}&appid={_apiKey}";

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
            using HttpClient client = clientFactory.CreateClient();

            string requestUri = $"{_weatherApiUrl}/data/2.5/weather?lat={cityCoordinates.lat}&lon={cityCoordinates.lon}&appid={_apiKey}";

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
