using CityWeatherApp.Cities;
using CityWeatherApp.DAL.Cities;
using CityWeatherApp.Domain;
using CityWeatherApp.ThirdParty;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityWeatherApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CityWeatherController : ControllerBase
    {
        private ICityDal cityDal;
        private ICountriesClient countriesClient;
        private IOpenWeatherClient openWeatherClient;
        private ICityResponseBuilder cityResponseBuilder;

        public CityWeatherController(
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

        [HttpPost()]
        public IActionResult AddCity(AddCityRequest request)
        {
            cityDal.AddCity(request);
            return NoContent();
        }

        [HttpGet()]
        public async Task<List<CityResponse>> SearchCity(string name)
        {
            List<CityRecord> cityRecords = cityDal.SearchByName(name);

            if (cityRecords.Count == 0)
            {
                return new List<CityResponse>();
            }

            List<GetCountryByNameResponse> countryResponse = await countriesClient.GetByName(name);
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

        [HttpPut("{id:int}")]
        public IActionResult UpdateById(int id, UpdateCityRequest request)
        {
            CityRecord existingCity = cityDal.GetById(id);

            if (existingCity == null)
            {
                return NotFound();
            }

            cityDal.UpdateById(id, request);

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteById(int id)
        {
            CityRecord existingCity = cityDal.GetById(id);

            if (existingCity == null)
            {
                return NotFound();
            }

            cityDal.DeleteById(id);

            return Ok();
        }
    }
}
