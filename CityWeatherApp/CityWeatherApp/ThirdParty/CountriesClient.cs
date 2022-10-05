using CityWeatherApp.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CityWeatherApp.ThirdParty
{
    public class CountriesClient : ICountriesClient
    {
        private IHttpClientFactory clientFactory;
        private string url;

        public CountriesClient(IHttpClientFactory clientFactory, IOptions<ExternalApiOptions> configuration)
        {
            this.clientFactory = clientFactory;
            this.url = configuration.Value.CountryApiUrl;
        }

        public async Task<List<GetCountryByNameResponse>> GetByName(string name)
        {
            using HttpClient client = clientFactory.CreateClient();

            string requestUri = $"https://{url}/v3.1/name/{name}";

            HttpResponseMessage response = await client.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            using var responseStream = await response.Content.ReadAsStreamAsync();
            List<GetCountryByNameResponse> result = await JsonSerializer.DeserializeAsync<List<GetCountryByNameResponse>>(responseStream);

            return result;
        }
    }

    public interface ICountriesClient
    {
        public Task<List<GetCountryByNameResponse>> GetByName(string name);
    }

    public class GetCountryByNameResponse
    {
        public string cca2 { get; set; }
        public string cca3 { get; set; }

        public Dictionary<string, Currency> currencies { get; set; }
    }

    public class Currency
    {
        public string name { get; set; }
    }
}
