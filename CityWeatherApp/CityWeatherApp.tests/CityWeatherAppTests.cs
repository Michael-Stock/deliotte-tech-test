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

            controller.AddCity(CreateTestCity());

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
        public async Task SearchCityByName_NotFound_ReturnsEmpty()
        {
            CityWeatherController controller = CreateController();

            controller.AddCity(CreateTestCity());

            List<CityResponse> cities = await controller.SearchCity("Paris");

            Assert.AreEqual(0, cities.Count);
        }

        [TestMethod]
        public async Task DeleteCityById_Success_DeletesCity() 
        {
            CityWeatherController controller = CreateController();

            controller.AddCity(CreateTestCity());

            List<CityResponse> cities = await controller .SearchCity("New York");
            CityResponse city = cities[0];

            controller.DeleteById((int)city.Id);

            List<CityResponse> result = await controller .SearchCity("New York");

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void DeleteCityById_NotFound_ReturnsNotFound()
        {
            CityWeatherController controller = CreateController();

            NotFoundResult result = controller.DeleteById(1) as NotFoundResult;

            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public async Task UpdateCityById_Success_UpdatesCityById()
        {
            CityWeatherController controller = CreateController();

            controller.AddCity(CreateTestCity());

            List<CityResponse> cities = await controller .SearchCity("New York");
            CityResponse city = cities[0];

            UpdateCityRequest request = new UpdateCityRequest()
            {
                TouristRating = 1,
                EstimatedPopulation = 1000000,
                DateEstablished = new DateTime(2022, 10, 1)
            };

            controller.UpdateById((int)city.Id, request);

            List<CityResponse> results = await controller .SearchCity("New York");
            CityResponse result = results[0];

            Assert.AreEqual(1, result.TouristRating);
            Assert.AreEqual(1000000, result.EstimatedPopulation);
            Assert.AreEqual(new DateTime(2022, 10, 1), result.DateEstablished);
        }

        [TestMethod]
        public void UpdateCityById_NotFound_ReturnsNotFound()
        {
            CityWeatherController controller = CreateController();

            NotFoundResult result = controller.UpdateById(1, null) as NotFoundResult;

            Assert.AreEqual(404, result.StatusCode);
        }

        public AddCityRequest CreateTestCity()
        {
            DateTime cityDate = new DateTime(2001, 05, 01);

            AddCityRequest result = new AddCityRequest()
            {
                Name = "New York",
                State = "New York",
                TouristRating = 5,
                Country = "USA",
                DateEstablished = cityDate,
                EstimatedPopulation = 23000,
            };

            return result;
        }

        public CityWeatherController CreateController()
        {
            Mock<ICountriesClient> countriesClient = new Mock<ICountriesClient>();
            countriesClient
                .Setup(m => m.GetByName(It.IsAny<string>()))
                .ReturnsAsync(
                    new List<GetCountryByNameResponse>()
                    {
                        new GetCountryByNameResponse()
                        {
                            cca2 = "US",
                            cca3 = "USA",
                            currencies = new Dictionary<string, Currency>()
                            {
                                {
                                    "USD", new Currency()
                                    {
                                        name = "US Dollar"
                                    }
                                }
                            }
                        }
                    });

            Mock<IOpenWeatherClient> openWeatherClient = new Mock<IOpenWeatherClient>();
            openWeatherClient
                .Setup(m => m.GetCityWeather(It.IsAny<string>()))
                .ReturnsAsync(
                    new CityWeatherResponse()
                    {
                        weather = new List<CityWeatherEntry>()
                        {
                            new CityWeatherEntry()
                            {
                                main = "Rain"
                            }
                        }
                    }
                );

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
