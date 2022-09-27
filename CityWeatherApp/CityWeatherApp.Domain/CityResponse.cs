using System;

namespace CityWeatherApp.Domain
{
    public class CityResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }

        public string Country { get; set; }

        public int TouristRating { get; set; }

        public DateTime DateEstablished { get; set; }

        public int EstimatedPopulation { get; set; }

        public string TwoDigitCountryCode { get; set; }

        public string ThreeDigitCountryCode { get; set; }

        public string Weather { get; set; }

        public string Currency { get; set; }
    }
}
