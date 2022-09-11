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

        public void AddCity(AddCityRequest request)
        {
            cityDal.AddCity(request);
        }

        public async Task<List<CityResponse>> SearchCity(string name)
        {
            List<CityRecord> cityRecords = cityDal.SearchByName(name);

            if (cityRecords.Count == 0)
            {
                return new List<CityResponse>();
            }

            List<GetCountryByNameResponse> countryResponse = await countriesClient.GetByName(cityRecords.First().Country);
            CityWeatherResponse weatherResponse = await openWeatherClient.GetCityWeather(name);

            CityResponseBuilderParams parameters = new CityResponseBuilderParams()
            {
                CityRecord = cityRecords.First(),
                CountryResponse = countryResponse,
                CityWeatherResponse = weatherResponse
            };

            List<CityResponse> results = new List<CityResponse>()
            {
                cityResponseBuilder.Build(parameters)
            };

            return results;
        }

        public void UpdateById(int id, UpdateCityRequest request)
        {
            CityRecord existingCity = cityDal.GetById(id);

            if (existingCity == null)
            {
                throw new Exception("City not found");
            }

            cityDal.UpdateById(id, request);
        }

        public void DeleteById(int id)
        {
            CityRecord existingCity = cityDal.GetById(id);

            if (existingCity == null)
            {
                throw new Exception("City not found");
            }

            cityDal.DeleteById(id);
        }
    }

    public interface ICityService
    {
        public void AddCity(AddCityRequest request);
        public void UpdateById(int id, UpdateCityRequest request);
        public void DeleteById(int id);

        public Task<List<CityResponse>> SearchCity(string name);
    }
}
