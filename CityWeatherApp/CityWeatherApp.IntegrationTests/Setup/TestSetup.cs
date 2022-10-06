using CityWeatherApp.Configuration;
using CityWeatherApp.DAL.Cities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CityWeatherApp.IntegrationTests.Setup
{
    public static class TestSetup
    {
        public static void CleanDatabase()
        {
            using (CityContext context = new CityContext())
            {
                context.Cities.RemoveRange(context.Cities);
                context.SaveChanges();
            }
        }

        public static WebApplicationFactory<Startup> CreateApp()
        {
            return new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices((IServiceCollection services) =>
                    {
                        services.Configure<ExternalApiOptions>(config =>
                        {
                            config.CountryApiUrl = "http://localhost:3005";
                            config.WeatherApiUrl = "http://localhost:3005";
                            config.GeoApiUrl = "http://localhost:3005";
                            config.WeatherApiKey = "123";
                        });
                    });
                });
        }
    }
}
