using CityWeatherApp.Cities;
using CityWeatherApp.DAL.Cities;
using CityWeatherApp.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CityWeatherApp.tests
{
    [TestClass]
    public class CityResponseBuilderTests
    {
        [TestMethod]
        public void Build_AllParameters_ReturnsCityWithAllData()
        {
            CityResponseBuilder builder = new CityResponseBuilder();

            CityResponseBuilderParams parameters = new CityResponseBuilderParams()
            {
                CityRecord = CreateTestCityRecord(),
                CountryResponse = CityMockData.CreateUsaCountryResponse(),
                CityWeatherResponse = CityMockData.CreateNewYorkWeather()

            };

            CityEntry result = builder.Build(parameters);
            
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("New York", result.Name);
            Assert.AreEqual("New York", result.State);
            Assert.AreEqual(5, result.TouristRating);
            Assert.AreEqual("USA", result.Country);
            Assert.AreEqual(new DateTime(2001, 05, 01), result.DateEstablished);
            Assert.AreEqual(23000, result.EstimatedPopulation);
            Assert.AreEqual("US", result.TwoDigitCountryCode);
            Assert.AreEqual("USA", result.ThreeDigitCountryCode);
            Assert.AreEqual("Rain", result.Weather);
            Assert.AreEqual("US Dollar", result.Currency);
        }

        [TestMethod]
        public void Build_ExternalApiResponsesNull_ReturnsNullForMissingValues()
        {
            CityResponseBuilder builder = new CityResponseBuilder();

            CityResponseBuilderParams parameters = new CityResponseBuilderParams()
            {
                CityRecord = CreateTestCityRecord(),
                CountryResponse = null,
                CityWeatherResponse = null

            };

            CityEntry result = builder.Build(parameters);

            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("New York", result.Name);
            Assert.AreEqual("New York", result.State);
            Assert.AreEqual(5, result.TouristRating);
            Assert.AreEqual("USA", result.Country);
            Assert.AreEqual(new DateTime(2001, 05, 01), result.DateEstablished);
            Assert.AreEqual(23000, result.EstimatedPopulation);
            Assert.AreEqual(null, result.TwoDigitCountryCode);
            Assert.AreEqual(null, result.ThreeDigitCountryCode);
            Assert.AreEqual(null, result.Weather);
            Assert.AreEqual(null, result.Currency);
        }

        private CityRecord CreateTestCityRecord()
        {
            DateTime cityDate = new DateTime(2001, 05, 01);

            CityRecord result = new CityRecord()
            {
                Id = 1,
                Name = "New York",
                State = "New York",
                TouristRating = 5,
                Country = "USA",
                DateEstablished = cityDate,
                EstimatedPopulation = 23000,
            };

            return result;
        }
    }
}
