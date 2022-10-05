using FluentAssertions;
using TechTalk.SpecFlow;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CityWeatherApp.MsTestApi.Common;
using System.Text;
using System.Net.Mime;

namespace CityWeatherApp.MsTestApi.Steps
{
    [Binding]
    public sealed class CommonSteps
    {
        private readonly ItemContext _itemContext;

        public CommonSteps(ItemContext itemContext)
        {
            _itemContext = itemContext;
        }

        [Given(@"a ""(.*)"" loaded from ""(.*)""")]
        public void GivenALoadedFrom(string contextName, string filePath)
        {
            var fullFilePath = Path.Combine("../../../Features/", filePath);
            var data = File.ReadAllText(fullFilePath);
            var result = JsonConvert.DeserializeObject(data) as JObject;

            if (result == null)
                throw new Exception($"Failed to deserialize JSON from file {fullFilePath}");

            _itemContext.JsonLookup.Add(contextName, result);
        }
    }
}
