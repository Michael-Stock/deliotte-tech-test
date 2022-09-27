using CityWeatherApp.ThirdParty;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CityWeatherApp.IntegrationTests.Mocks
{
    public class MockCountriesClient : ICountriesClient
    {
        public Task<List<GetCountryByNameResponse>> GetByName(string name)
        {
            if (name == "USA") 
            {
                return Task.FromResult(CityMockData.CreateUsaCountryResponse());
            }

            if (name == "United Kingdom")
            {
                return Task.FromResult(CityMockData.CreateUkCountryResponse());
            }

            return Task.FromResult(new List<GetCountryByNameResponse>());
        }
    }
}
