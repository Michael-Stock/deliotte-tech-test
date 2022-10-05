using CityWeatherApp.Cities;
using CityWeatherApp.Domain;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CityWeatherApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CityWeatherController : ControllerBase
    {
        ICityService _cityService;
        IValidator<AddCityRequest> _addCityRequestValidator;

        public CityWeatherController(
            ICityService cityService,
            IValidator<AddCityRequest> addCityRequestValidator)
        {
            this._cityService = cityService;
            this._addCityRequestValidator = addCityRequestValidator;
        }

        [HttpPost()]
        public async Task<IActionResult> AddCity(AddCityRequest request)
        {
            var validationResult = await _addCityRequestValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest();
            }

            await _cityService.AddCity(request);
            return NoContent();
        }

        [HttpGet()]
        public async Task<CityResponse> SearchCity(string name)
        {
            CityResponse results = await _cityService.SearchCity(name);

            return results;
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateById(int id, UpdateCityRequest request)
        {
            try
            {
                await _cityService.UpdateById(id, request);
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
                await _cityService.DeleteById(id);
            }
            catch
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
