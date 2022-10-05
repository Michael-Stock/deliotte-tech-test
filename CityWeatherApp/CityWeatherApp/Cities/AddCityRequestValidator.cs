using CityWeatherApp.Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityWeatherApp.Cities
{
    public class AddCityRequestValidator : AbstractValidator<AddCityRequest>
    {
        public AddCityRequestValidator()
        {
            RuleFor(r => r.Name).NotEmpty();
        }
    }
}
