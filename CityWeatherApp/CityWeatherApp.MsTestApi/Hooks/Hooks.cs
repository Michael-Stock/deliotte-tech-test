using BoDi;
using CityWeatherApp.DAL.Cities;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using TechTalk.SpecFlow;

namespace CityWeatherApp.MsTestApi.Hooks
{
    [Binding]
    public class Hooks
    {
        private IObjectContainer _objectContainer;
        private static WebApplicationFactory<Startup>? _application;
        private static HttpClient? _client;

        public Hooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            using (CityContext context = new CityContext())
            {
                context.Cities.RemoveRange(context.Cities);
                context.SaveChanges();
            }

            _application = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                });

            _client = _application.CreateClient();
            _client.DefaultRequestHeaders.Add(HttpRequestHeader.Accept.ToString(), MediaTypeNames.Application.Json);
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            if (_application != null)
                _application.Dispose();

            if (_client != null)
                _client.Dispose();
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            if (_client != null)
                _objectContainer.RegisterInstanceAs(_client, typeof(HttpClient));
        }
    }
}
