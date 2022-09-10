using CityWeatherApp.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace CityWeatherApp.DAL.Cities
{
    public class CityContext : DbContext
    {
        public string DbPath { get; }

        public CityContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<CityRecord> Cities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=cities.db");
    }

    public class CityRecord
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public int TouristRating { get; set; }

        public DateTime DateEstablished { get; set; }

        public int EstimatedPopulation { get; set; }
    }
}
