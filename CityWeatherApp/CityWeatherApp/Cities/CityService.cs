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
        private readonly ICityDal _cityDal;
        private readonly ICountriesClient _countriesClient;
        private readonly IOpenWeatherClient _openWeatherClient;
        private readonly ICityResponseBuilder _cityResponseBuilder;

        public CityService(
            ICityDal cityDal,
            ICountriesClient countriesClient,
            IOpenWeatherClient openWeatherClient,
            ICityResponseBuilder cityResponseBuilder)
        {
            _cityDal = cityDal;
            _countriesClient = countriesClient;
            _openWeatherClient = openWeatherClient;
            _cityResponseBuilder = cityResponseBuilder;
        }

        public async Task AddCity(AddCityRequest request)
        {
            await _cityDal.AddCity(request);
        }

        public async Task<List<CityResponse>> SearchCity(string name)
        {
            List<CityRecord> cityRecords = await _cityDal.SearchByName(name);

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
            CityRecord existingCity = await _cityDal.GetById(id);

            if (existingCity == null)
            {
                throw new Exception("City not found");
            }

            await _cityDal.UpdateById(id, request);
        }

        public async Task DeleteById(int id)
        {
            CityRecord existingCity = await _cityDal.GetById(id);

            if (existingCity == null)
            {
                throw new Exception("City not found");
            }

            await _cityDal.DeleteById(id);
        }

        private async Task<CityResponse> BuildCity(CityRecord cityRecord)
        {
            // Could be done in parallel but I will need the result of the country response to get the right city later
            List<GetCountryByNameResponse> countryResponse = await _countriesClient.GetByName(cityRecord.Country);
            CityWeatherResponse weatherResponse = await _openWeatherClient.GetCityWeather(cityRecord.Name);

            CityResponseBuilderParams parameters = new CityResponseBuilderParams()
            {
                CityRecord = cityRecord,
                CountryResponse = countryResponse,
                CityWeatherResponse = weatherResponse
            };

            CityResponse result = _cityResponseBuilder.Build(parameters);

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
