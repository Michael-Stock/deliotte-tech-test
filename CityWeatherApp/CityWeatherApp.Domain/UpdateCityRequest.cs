using System;
using System.Collections.Generic;
using System.Text;

namespace CityWeatherApp.Domain
{
    public class UpdateCityRequest
    {
        public int TouristRating { get; set; }

        public DateTime DateEstablished { get; set; }

        public int EstimatedPopulation { get; set; }
    }
}
