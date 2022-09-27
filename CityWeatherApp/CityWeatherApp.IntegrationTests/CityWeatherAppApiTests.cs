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

            Assert.AreEqual("New York", city.Name);
            Assert.AreEqual("New York", city.State);
            Assert.AreEqual(5, city.TouristRating);
            Assert.AreEqual("USA", city.Country);
            Assert.AreEqual(new DateTime(2001, 05, 01), city.DateEstablished);
            Assert.AreEqual(23000, city.EstimatedPopulation);
            Assert.AreEqual("US", city.TwoDigitCountryCode);
            Assert.AreEqual("USA", city.ThreeDigitCountryCode);
            Assert.AreEqual("Rain", city.Weather);
            Assert.AreEqual("US Dollar", city.Currency);
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

            Assert.AreEqual(2, results.Count);

            CityResponse newportUk = results[0];

            Assert.AreEqual("Newport", newportUk.Name);
            Assert.AreEqual("Gwent", newportUk.State);
            Assert.AreEqual(2, newportUk.TouristRating);
            Assert.AreEqual("United Kingdom", newportUk.Country);
            Assert.AreEqual(new DateTime(2001, 05, 01), newportUk.DateEstablished);
            Assert.AreEqual(30000, newportUk.EstimatedPopulation);
            Assert.AreEqual("GB", newportUk.TwoDigitCountryCode);
            Assert.AreEqual("GBR", newportUk.ThreeDigitCountryCode);
            Assert.AreEqual("Sunshine", newportUk.Weather);
            Assert.AreEqual("British pound", newportUk.Currency);

            CityResponse newportUsa = results[1];

            Assert.AreEqual("Newport", newportUsa.Name);
            Assert.AreEqual("Rhode Island", newportUsa.State);
            Assert.AreEqual(5, newportUsa.TouristRating);
            Assert.AreEqual("USA", newportUsa.Country);
            Assert.AreEqual(new DateTime(2001, 05, 01), newportUsa.DateEstablished);
            Assert.AreEqual(50000, newportUsa.EstimatedPopulation);
            Assert.AreEqual("US", newportUsa.TwoDigitCountryCode);
            Assert.AreEqual("USA", newportUsa.ThreeDigitCountryCode);
            Assert.AreEqual("Sunshine", newportUsa.Weather);
            Assert.AreEqual("US Dollar", newportUsa.Currency);
        }

        [TestMethod]
        public async Task SearchCityByName_NotFound_ReturnsEmpty()
        {
            await AddCity(CityMockData.CreateNewYorkCity());

            var result = await GetByName("Paris");

            Assert.AreEqual(0, result.Count);
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

            Assert.AreEqual(0, result.Count);
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

            Assert.AreEqual(1, result.TouristRating);
            Assert.AreEqual(1000000, result.EstimatedPopulation);
            Assert.AreEqual(new DateTime(2022, 10, 1), result.DateEstablished);
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
    }
}
