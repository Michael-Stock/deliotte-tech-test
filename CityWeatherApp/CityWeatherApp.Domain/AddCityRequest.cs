using System;
using System.Collections.Generic;
using System.Text;

namespace CityWeatherApp.Domain
{
    public class AddCityRequest
    {
        public string Name { get; set; }
        public string State { get; set; }

        public string Country { get; set; }

        public int TouristRating { get; set; }

        public DateTime DateEstablished { get; set; }

        public int EstimatedPopulation { get; set; }
    }
}
