using CityWeatherApp.Domain;
using CityWeatherApp.ThirdParty;
using System;
using System.Collections.Generic;
using System.Text;

namespace CityWeatherApp.tests
{
    public class CityMockData
    {
        public static List<GetCountryByNameResponse> CreateUsaCountryResponse()
        {
            List<GetCountryByNameResponse> result = new List<GetCountryByNameResponse>()
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
            };

            return result;
        }

        public static List<GetCountryByNameResponse> CreateUkCountryResponse()
        {
            List<GetCountryByNameResponse> result = new List<GetCountryByNameResponse>()
            {
                new GetCountryByNameResponse()
                {
                    cca2 = "GB",
                    cca3 = "GBR",
                    currencies = new Dictionary<string, Currency>()
                    {
                        {
                            "GBP", new Currency()
                            {
                                name = "British pound"
                            }
                        }
                    }
                }
            };

            return result;
        }

        public static CityWeatherResponse CreateNewYorkWeather()
        {
            CityWeatherResponse result = new CityWeatherResponse()
            {
                weather = new List<CityWeatherEntry>()
                {
                    new CityWeatherEntry()
                    {
                        main = "Rain"
                    }
                }
            };

            return result;
        }

        public static CityWeatherResponse CreateNewportWeather()
        {
            CityWeatherResponse result = new CityWeatherResponse()
            {
                weather = new List<CityWeatherEntry>()
                {
                    new CityWeatherEntry()
                    {
                        main = "Sunshine"
                    }
                }
            };

            return result;
        }

        public static AddCityRequest CreateNewYorkCity()
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

        public static List<AddCityRequest> CreateNewportCities()
        {
            DateTime cityDate = new DateTime(2001, 05, 01);

            List<AddCityRequest> results = new List<AddCityRequest>()
            {
                new AddCityRequest()
                {
                    Name = "Newport",
                    State = "Gwent",
                    TouristRating = 2,
                    Country = "United Kingdom",
                    DateEstablished = cityDate,
                    EstimatedPopulation = 30000,
                },
                new AddCityRequest()
                {
                    Name = "Newport",
                    State = "Rhode Island",
                    TouristRating = 5,
                    Country = "USA",
                    DateEstablished = cityDate,
                    EstimatedPopulation = 50000
                }
            };

            return results;
        }
    }
}
