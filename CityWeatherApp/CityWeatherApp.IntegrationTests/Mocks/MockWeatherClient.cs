using CityWeatherApp.ThirdParty;
using System.Threading.Tasks;

namespace CityWeatherApp.IntegrationTests.Mocks
{
    public class MockWeatherClient : IOpenWeatherClient
    {
        public Task<CityWeatherResponse> GetCityWeather(string name)
        {
            if (name == "New York")
            {
                return Task.FromResult(CityMockData.CreateNewYorkWeather());
            }

            if (name == "Newport")
            {
                return Task.FromResult(CityMockData.CreateNewportWeather());
            }

            return Task.FromResult(new CityWeatherResponse());
        }
    }
}
