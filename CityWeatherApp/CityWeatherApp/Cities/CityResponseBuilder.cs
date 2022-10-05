using CityWeatherApp.DAL.Cities;
using CityWeatherApp.Domain;
using CityWeatherApp.ThirdParty;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CityWeatherApp.Cities
{
    public class CityResponseBuilder : ICityResponseBuilder
    {
        public CityEntry Build(CityResponseBuilderParams parameters)
        {
            CityEntry result = new CityEntry()
            {
                Id = parameters.CityRecord.Id,
                State = parameters.CityRecord.State,
                TouristRating = parameters.CityRecord.TouristRating,
                Name = parameters.CityRecord.Name,
                Country = parameters.CityRecord.Country,
                DateEstablished = parameters.CityRecord.DateEstablished,
                EstimatedPopulation = parameters.CityRecord.EstimatedPopulation,
                TwoDigitCountryCode = parameters.CountryResponse?.First().cca2,
                ThreeDigitCountryCode = parameters.CountryResponse?.First().cca3,
                Currency = parameters.CountryResponse?.First().currencies.First().Value.name,
                Weather = parameters.CityWeatherResponse?.weather.First().main,
            };

            return result;
        }
    }

    public interface ICityResponseBuilder
    {
        public CityEntry Build(CityResponseBuilderParams parameters);
    }

    public class CityResponseBuilderParams
    {
        public CityRecord CityRecord { get; set; }
        public List<GetCountryByNameResponse> CountryResponse { get; set; }
        public CityWeatherResponse CityWeatherResponse { get; set; }
    }
}
