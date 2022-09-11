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
        ICityService cityService;

        public CityWeatherController(ICityService cityService)
        {
            this.cityService = cityService;
        }

        /// <summary>
        /// Adds a city
        /// </summary>
        /// <param name="request">The add city request</param>
        /// <returns></returns>
        [HttpPost()]
        public IActionResult AddCity(AddCityRequest request)
        {
            cityService.AddCity(request);
            return NoContent();
        }

        [HttpGet()]
        public async Task<List<CityResponse>> SearchCity(string name)
        {
            List<CityResponse> results = await cityService.SearchCity(name);

            return results;
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateById(int id, UpdateCityRequest request)
        {
            try
            {
                cityService.UpdateById(id, request);
            }
            catch
            {
                return NotFound();
            }


            return Ok();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteById(int id)
        {
            try
            {
                cityService.DeleteById(id);
            }
            catch
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
