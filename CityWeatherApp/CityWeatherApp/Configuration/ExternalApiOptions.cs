using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityWeatherApp.Configuration
{
    public class ExternalApiOptions
    {
        public const string ExternalApi = "ExternalApi";
        public string CountryApiUrl { get; set; }
        public string WeatherApiUrl { get; set; }
        public string WeatherApiKey { get; set; }
    }
}
