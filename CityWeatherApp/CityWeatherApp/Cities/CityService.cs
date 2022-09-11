using CityWeatherApp.DAL.Cities;
using CityWeatherApp.Domain;
using CityWeatherApp.ThirdParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityWeatherApp.Cities
{
    public class CityService : ICityService
    {
        private ICityDal cityDal;
        private ICountriesClient countriesClient;
        private IOpenWeatherClient openWeatherClient;
        private ICityResponseBuilder cityResponseBuilder;

        public CityService(
            ICityDal cityDal,
            ICountriesClient countriesClient,
            IOpenWeatherClient openWeatherClient,
            ICityResponseBuilder cityResponseBuilder)
        {
            this.cityDal = cityDal;
            this.countriesClient = countriesClient;
            this.openWeatherClient = openWeatherClient;
            this.cityResponseBuilder = cityResponseBuilder;
        }

        public async Task AddCity(AddCityRequest request)
        {
            await cityDal.AddCity(request);
        }

        public async Task<List<CityResponse>> SearchCity(string name)
        {
            List<CityRecord> cityRecords = await cityDal.SearchByName(name);

            if (cityRecords.Count == 0)
            {
                return new List<CityResponse>();
            }

            var tasks = cityRecords.Select(city => BuildCity(city));
            var results = await Task.WhenAll(tasks);

            return results.ToList();
        }

        public async Task UpdateById(int id, UpdateCityRequest request)
        {
            CityRecord existingCity = await cityDal.GetById(id);

            if (existingCity == null)
            {
                throw new Exception("City not found");
            }

            await cityDal.UpdateById(id, request);
        }

        public async Task DeleteById(int id)
        {
            CityRecord existingCity = await cityDal.GetById(id);

            if (existingCity == null)
            {
                throw new Exception("City not found");
            }

            await cityDal.DeleteById(id);
        }

        private async Task<CityResponse> BuildCity(CityRecord cityRecord)
        {
            List<GetCountryByNameResponse> countryResponse = await countriesClient.GetByName(cityRecord.Country);
            CityWeatherResponse weatherResponse = await openWeatherClient.GetCityWeather(cityRecord.Name);

            CityResponseBuilderParams parameters = new CityResponseBuilderParams()
            {
                CityRecord = cityRecord,
                CountryResponse = countryResponse,
                CityWeatherResponse = weatherResponse
            };

            CityResponse result = cityResponseBuilder.Build(parameters);

            return result;
        }
    }

    public interface ICityService
    {
        public Task AddCity(AddCityRequest request);
        public Task UpdateById(int id, UpdateCityRequest request);
        public Task DeleteById(int id);

        public Task<List<CityResponse>> SearchCity(string name);
    }
}
