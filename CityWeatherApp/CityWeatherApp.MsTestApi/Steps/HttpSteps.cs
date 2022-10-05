using CityWeatherApp.MsTestApi.Common;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace CityWeatherApp.MsTestApi.Steps
{
    [Binding]
    public sealed class HttpSteps
    {
        private readonly ItemContext _itemContext;
        private readonly HttpClient _httpClient;

        public HttpSteps(
            ItemContext itemContext,
            HttpClient httpClient)
        {
            _itemContext = itemContext;
            _httpClient = httpClient;
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
    }
}
