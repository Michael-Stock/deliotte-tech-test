using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CityWeatherApp.ThirdParty
{
    public class OpenWeatherClient : IOpenWeatherClient
    {
        private IHttpClientFactory clientFactory;

        public OpenWeatherClient(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public async Task<CityWeatherResponse> GetCityWeather(string name)
        {
            HttpClient client = clientFactory.CreateClient();

            string requestUri = $"https://api.openweathermap.org/data/2.5/weather?lat={name}appid=1234";

            HttpResponseMessage response = await client.GetAsync(requestUri);

            using var responseStream = await response.Content.ReadAsStreamAsync();
            CityWeatherResponse result = await JsonSerializer.DeserializeAsync<CityWeatherResponse>(responseStream);

            return result;
        }
    }

    public interface IOpenWeatherClient
    {
        public Task<CityWeatherResponse> GetCityWeather(string name);
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
