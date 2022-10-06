using System;
using System.Collections.Generic;
using System.Text;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace CityWeatherApp.IntegrationTests.Setup
{
    public static class MockServerSetup
    {
        public static void SetupMockResponses(WireMockServer server)
        {
            SetupCountryMockResponses(server);
            SetupGeoLocationMockResponses(server);
            SetupWeatherResponses(server);
        }

        private static void SetupCountryMockResponses(WireMockServer server)
        {
            server
                .Given(Request.Create().WithPath("/v3.1/name/USA").UsingGet())
                .RespondWith(Response.Create().WithStatusCode(200).WithBodyAsJson(CityMockData.CreateUsaCountryResponse()));

            server
                .Given(Request.Create().WithPath("/v3.1/name/United Kingdom").UsingGet())
                .RespondWith(Response.Create().WithStatusCode(200).WithBodyAsJson(CityMockData.CreateUkCountryResponse()));
        }

        private static void SetupGeoLocationMockResponses(WireMockServer server)
        {
            server
                .Given(Request.Create()
                    .WithPath("/geo/1.0/direct")
                    .WithParam("q", new ExactMatcher("New York"))
                    .WithParam("appid", "123")
                    .UsingGet())
                .RespondWith(Response.Create().WithStatusCode(200).WithBodyAsJson(CityMockData.CreateNewYorkGeoData()));

            server
                .Given(Request.Create().WithPath("/geo/1.0/direct")
                    .WithParam("q", new ExactMatcher("Newport"))
                    .WithParam("appid", "123")
                    .UsingGet())
                .RespondWith(Response.Create().WithStatusCode(200).WithBodyAsJson(CityMockData.CreateNewportGeoData()));
        }

        private static void SetupWeatherResponses(WireMockServer server)
        {
            server
                .Given(Request.Create()
                    .WithPath("/data/2.5/weather")
                    .WithParam("lat", new ExactMatcher("10.0"))
                    .WithParam("lon", new ExactMatcher("20.0"))
                    .WithParam("appid", new ExactMatcher("123"))
                    .UsingGet())
                .RespondWith(Response.Create().WithStatusCode(200).WithBodyAsJson(CityMockData.CreateNewYorkWeather()));

            server
                .Given(Request.Create().WithPath("/data/2.5/weather")
                    .WithParam("lat", new ExactMatcher("100.0"))
                    .WithParam("lon", new ExactMatcher("200.0"))
                    .WithParam("appid", new ExactMatcher("123"))
                    .UsingGet())
                .RespondWith(Response.Create().WithStatusCode(200).WithBodyAsJson(CityMockData.CreateNewportWeather()));
        }
    }
}
