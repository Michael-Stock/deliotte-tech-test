using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CityWeatherApp.MsTestApi.Common
{
    public class ItemContext
    {
        public ItemContext()
        {
            JsonLookup = new Dictionary<string, JObject>();
        }

        public Dictionary<string, JObject> JsonLookup { get; set; }

        public int StatusCode { get; set; }
    }
}
