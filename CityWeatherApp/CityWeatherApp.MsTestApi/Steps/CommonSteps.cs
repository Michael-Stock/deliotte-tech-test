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
        private readonly HttpClient _httpClient;

        public CommonSteps(
            ItemContext itemContext,
            HttpClient httpClient)
        {
            _itemContext = itemContext;
            _httpClient = httpClient;
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

        [StepDefinition(@"that ""(.*)"" contains the following properties")]
        public void GivenThatContainsTheFollowingProperties(string contextName, Table table)
        {
            var contextValue = _itemContext.JsonLookup[contextName];

            if (contextValue == null)
                throw new Exception($"Could not resolve {contextName} in the context");

            foreach (var row in table.Rows)
            {
                var propertyName = row.ElementAt(0).Value;
                var propertyValue = row.ElementAt(1).Value;

                var actual = contextValue.GetValue(propertyName);

                if (actual == null)
                    throw new Exception($"Could not find {propertyName} on the object");

                var actualValue = actual.ToString();
                actualValue.Should().Be(propertyValue);
            }
        }

        [StepDefinition(@"that ""(.*)"" contains the following instances")]
        public void GivenThatContainsTheFollowingInstances(string contextName, Table table)
        {
            var contextValue = GetByPropertyPath(_itemContext, contextName);

            var arrayObject = contextValue as JArray;

            // First column is the match column
            var matchHeader = table.Header.ElementAt(0);

            foreach (var row in table.Rows)
            {
                var matchingEntry = arrayObject.Children().Where(c => c[matchHeader].ToString() == row.Values.ElementAt(0)).First();

                foreach (var header in table.Header)
                {
                    var expected = row[header];
                    var actual = matchingEntry[header].ToString();

                    actual.Should().Be(expected);
                }
            }
        }

        [StepDefinition(@"I POST that ""(.*)"" to ""(.*)"" and store the ""(.*)""")]
        public async Task PostAndStoreTheResponse(string requestContextName, string url, string responseContextName)
        {
            var jsonObject = _itemContext.JsonLookup[requestContextName];
            var payload = JsonConvert.SerializeObject(jsonObject);

            var content = new StringContent(payload, Encoding.UTF8, MediaTypeNames.Application.Json);

            var response = await _httpClient.PostAsync(url, content);

            var responseData = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject(responseData) as JObject;

            _itemContext.StatusCode = (int)response.StatusCode;

            if (responseObject != null)
                _itemContext.JsonLookup[responseContextName] = responseObject;
        }

        [StepDefinition(@"the status code is (.*)")]
        public void TheStatusCodeIs(int expected)
        {
            _itemContext.StatusCode.Should().Be(expected);
        }

        [Given(@"I GET ""(.*)"" and store the ""(.*)""")]
        public async Task GetAndStoreTheResponse(string url, string responseContextName)
        {
            var response = await _httpClient.GetAsync(url);

            var responseData = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject(responseData) as JObject;

            if (responseObject != null)
                _itemContext.JsonLookup[responseContextName] = responseObject;

            _itemContext.StatusCode = (int)response.StatusCode;
        }

        private JToken GetByPropertyPath(ItemContext itemContext, string propertyPath)
        {
            var parts = propertyPath.Split(".");

            var result = itemContext.JsonLookup[parts[0]] as JToken;

            foreach (var part in parts.Skip(1))
            {
                result = result[part] as JToken;

                if (result == null)
                    throw new Exception($"Could not resolve {propertyPath} in the context");
            }

            return result;
        }
    }
}
