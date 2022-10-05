using System;
using System.Collections.Generic;
using System.Text;

namespace CityWeatherApp.Domain
{
    public class CityResponse
    {
        public CityResponse()
        {
            Cities = new List<CityEntry>();
        }

        public List<CityEntry> Cities { get; set; }
    }
}
