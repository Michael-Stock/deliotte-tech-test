using CityWeatherApp.Cities;
using CityWeatherApp.Controllers;
using CityWeatherApp.DAL.Cities;
using CityWeatherApp.Domain;
using CityWeatherApp.ThirdParty;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CityWeatherApp.tests
{
    [TestClass]
    public class CityWeatherAppTests
    {
        [TestInitialize]
        public void Startup()
        {
            using (CityContext context = new CityContext())
            {
                context.Cities.RemoveRange(context.Cities);
                context.SaveChanges();
            }
        }

        [TestMethod]
        public async Task SearchCityByName_Success_ReturnsMatch()
        {
            CityWeatherController controller = CreateController();

            await controller.AddCity(CityMockData.CreateNewYorkCity());

            List<CityResponse> cities = await controller.SearchCity("New York");

            Assert.AreEqual(1, cities.Count);

            CityResponse city = cities[0];

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
            CityWeatherController controller = CreateController();

            List<AddCityRequest> cities = CityMockData.CreateNewportCities();

            foreach (var city in cities)
            {
                await controller.AddCity(city);
            }

            List<CityResponse> results = await controller.SearchCity("Newport");

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
            CityWeatherController controller = CreateController();

            await controller .AddCity(CityMockData.CreateNewYorkCity());

            List<CityResponse> cities = await controller.SearchCity("Paris");

            Assert.AreEqual(0, cities.Count);
        }

        [TestMethod]
        public async Task DeleteCityById_Success_DeletesCity() 
        {
            CityWeatherController controller = CreateController();

            await controller .AddCity(CityMockData.CreateNewYorkCity());

            List<CityResponse> cities = await controller.SearchCity("New York");
            CityResponse city = cities[0];

            await controller.DeleteById((int)city.Id);

            List<CityResponse> result = await controller.SearchCity("New York");

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task DeleteCityById_NotFound_ReturnsNotFound()
        {
            CityWeatherController controller = CreateController();

            NotFoundResult result = await controller.DeleteById(1) as NotFoundResult;

            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public async Task UpdateCityById_Success_UpdatesCityById()
        {
            CityWeatherController controller = CreateController();

            await controller .AddCity(CityMockData.CreateNewYorkCity());

            List<CityResponse> cities = await controller.SearchCity("New York");
            CityResponse city = cities[0];

            UpdateCityRequest request = new UpdateCityRequest()
            {
                TouristRating = 1,
                EstimatedPopulation = 1000000,
                DateEstablished = new DateTime(2022, 10, 1)
            };

            await controller .UpdateById((int)city.Id, request);

            List<CityResponse> results = await controller.SearchCity("New York");
            CityResponse result = results[0];

            Assert.AreEqual(1, result.TouristRating);
            Assert.AreEqual(1000000, result.EstimatedPopulation);
            Assert.AreEqual(new DateTime(2022, 10, 1), result.DateEstablished);
        }

        [TestMethod]
        public async Task UpdateCityById_NotFound_ReturnsNotFound()
        {
            CityWeatherController controller = CreateController();

            NotFoundResult result = await controller.UpdateById(1, null) as NotFoundResult;

            Assert.AreEqual(404, result.StatusCode);
        }

        private CityWeatherController CreateController()
        {
            Mock<ICountriesClient> countriesClient = new Mock<ICountriesClient>();
            countriesClient
                .Setup(m => m.GetByName(It.Is<string>(s => s == "USA"))).ReturnsAsync(CityMockData.CreateUsaCountryResponse());
            
            countriesClient
                .Setup(m => m.GetByName(It.Is<string>(s => s == "United Kingdom"))).ReturnsAsync(CityMockData.CreateUkCountryResponse());

            Mock<IOpenWeatherClient> openWeatherClient = new Mock<IOpenWeatherClient>();
            openWeatherClient
                .Setup(m => m.GetCityWeather(It.Is<string>(s => s == "New York"))).ReturnsAsync(CityMockData.CreateNewYorkWeather());

            openWeatherClient
                .Setup(m => m.GetCityWeather(It.Is<string>(s => s == "Newport"))).ReturnsAsync(CityMockData.CreateNewportWeather());

            ICityService cityService = new CityService(
                new CityDal(),
                countriesClient.Object,
                openWeatherClient.Object,
                new CityResponseBuilder()
            );

            CityWeatherController result = new CityWeatherController(cityService);

            return result;
        }
    }
}
