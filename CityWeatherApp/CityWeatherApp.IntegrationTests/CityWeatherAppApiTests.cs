using CityWeatherApp.DAL.Cities;
using CityWeatherApp.Domain;
using CityWeatherApp.IntegrationTests.Mocks;
using CityWeatherApp.ThirdParty;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CityWeatherApp.IntegrationTests
{
    [TestClass]
    public class CityWeatherAppApiTests
    {
        private readonly string baseUrl = "/cityWeather";
        private WebApplicationFactory<Startup> _application;
        private HttpClient _client;

        [TestInitialize]
        public void Initialize()
        {
            using (CityContext context = new CityContext())
            {
                context.Cities.RemoveRange(context.Cities);
                context.SaveChanges();
            }

            _application = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder => 
                {
                    builder.ConfigureServices((IServiceCollection services) =>
                    {
                        services.AddScoped<ICountriesClient, MockCountriesClient>();
                        services.AddScoped<IOpenWeatherClient, MockWeatherClient>();
                    });
                });

            _client = _application.CreateClient();
            _client.DefaultRequestHeaders.Add(HttpRequestHeader.Accept.ToString(), MediaTypeNames.Application.Json);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _client.Dispose();
            _application.Dispose();
        }

        [TestMethod]
        public async Task SearchCityByName_Success_ReturnsMatch()
        {
            await AddCity(CityMockData.CreateNewYorkCity());

            var result = await GetByName("New York");

            result.Should().HaveCount(1);

            var city = result[0];

            AssertCity(city, new CityResponse()
            {
                Name = "New York",
                State = "New York",
                TouristRating = 5,
                Country = "USA",
                DateEstablished = new DateTime(2001, 05, 01),
                EstimatedPopulation = 23000,
                TwoDigitCountryCode = "US",
                ThreeDigitCountryCode = "USA",
                Weather = "Rain",
                Currency = "US Dollar"
            });
        }

        [TestMethod]
        public async Task SearchCityByName_Success_ReturnsMultipleMatchesByExactName()
        {
            List<AddCityRequest> cities = CityMockData.CreateNewportCities();

            foreach (var city in cities)
            {
                await AddCity(city);
            }

            var results = await GetByName("Newport");

            results.Count.Should().Be(2);

            // Two different asserion styles I think I prefer the first one
            CityResponse newportUk = results[0];

            AssertCity(newportUk, new CityResponse()
            {
                Name = "Newport",
                State = "Gwent",
                TouristRating = 2,
                Country = "United Kingdom",
                DateEstablished = new DateTime(2001, 05, 01),
                EstimatedPopulation = 30000,
                TwoDigitCountryCode = "GB",
                ThreeDigitCountryCode = "GBR",
                Weather = "Sunshine",
                Currency = "British pound"
            });

            CityResponse newportUsa = results[1];

            newportUsa.Name.Should().Be("Newport");
            newportUsa.State.Should().Be("Rhode Island");
            newportUsa.TouristRating.Should().Be(5);
            newportUsa.Country.Should().Be("USA");
            newportUsa.DateEstablished.Should().Be(new DateTime(2001, 05, 01));
            newportUsa.EstimatedPopulation.Should().Be(50000);
            newportUsa.TwoDigitCountryCode.Should().Be("US");
            newportUsa.ThreeDigitCountryCode.Should().Be("USA");
            newportUsa.Weather.Should().Be("Sunshine");
            newportUsa.Currency.Should().Be("US Dollar");
        }

        [TestMethod]
        public async Task SearchCityByName_NotFound_ReturnsEmpty()
        {
            await AddCity(CityMockData.CreateNewYorkCity());

            var result = await GetByName("Paris");

            result.Count.Should().Be(0);
        }

        [TestMethod]
        public async Task DeleteCityById_Success_DeletesCity()
        {
            await AddCity(CityMockData.CreateNewYorkCity());

            var cities = await GetByName("New York");
            CityResponse city = cities[0];

            var response = await _client.DeleteAsync($"{baseUrl}/{city.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await GetByName("New York");

            result.Count.Should().Be(0);
        }

        [TestMethod]
        public async Task DeleteCityById_NotFound_ReturnsNotFound()
        {
            var result = await _client.DeleteAsync($"{baseUrl}/1");
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task UpdateCityById_Success_UpdatesCityById()
        {
            await AddCity(CityMockData.CreateNewYorkCity());

            var cities = await GetByName("New York");

            CityResponse city = cities[0];

            UpdateCityRequest updateRequest = new UpdateCityRequest()
            {
                TouristRating = 1,
                EstimatedPopulation = 1000000,
                DateEstablished = new DateTime(2022, 10, 1)
            };

            string body = JsonSerializer.Serialize(updateRequest);

            var response = await _client.PutAsync($"{baseUrl}/{city.Id}", CreateJsonPayload(body));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var results = await GetByName("New York");
            CityResponse result = results[0];

            result.TouristRating.Should().Be(1);
            result.EstimatedPopulation.Should().Be(1000000);
            result.DateEstablished.Should().Be(new DateTime(2022, 10, 1));
        }

        [TestMethod]
        public async Task UpdateCityById_NotFound_ReturnsNotFound()
        {
            UpdateCityRequest updateRequest = new UpdateCityRequest()
            {
                TouristRating = 1,
                EstimatedPopulation = 1000000,
                DateEstablished = new DateTime(2022, 10, 1)
            };

            string body = JsonSerializer.Serialize(updateRequest);

            var result = await _client.PutAsync($"{baseUrl}/1", CreateJsonPayload(body));
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private async Task AddCity(AddCityRequest cityRequest)
        {
            string body = JsonSerializer.Serialize(cityRequest);

            var postResponse = await _client.PostAsync(baseUrl, CreateJsonPayload(body));
            postResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        private async Task<List<CityResponse>> GetByName(string name)
        {
            var response = await _client.GetAsync($"{baseUrl}?name={name}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var jsonString = await response.Content.ReadAsStringAsync();
            var results = JsonSerializer.Deserialize<List<CityResponse>>(jsonString);

            return results;
        }

        private StringContent CreateJsonPayload(string body)
        {
            return new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json);
        }

        private void AssertCity(CityResponse actual, CityResponse expected)
        {
            actual.Id.Should().BeGreaterThan(0);

            actual.Should().BeEquivalentTo(expected, options => options.Excluding(o => o.Id));
        }
    }
}
