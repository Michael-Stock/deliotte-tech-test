using CityWeatherApp.Cities;
using CityWeatherApp.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        [HttpPost()]
        public async Task<IActionResult> AddCity(AddCityRequest request)
        {
            await cityService.AddCity(request);
            return NoContent();
        }

        [HttpGet()]
        public async Task<List<CityResponse>> SearchCity(string name)
        {
            List<CityResponse> results = await cityService.SearchCity(name);

            return results;
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateById(int id, UpdateCityRequest request)
        {
            try
            {
                await cityService.UpdateById(id, request);
            }
            catch
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                await cityService.DeleteById(id);
            }
            catch
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
