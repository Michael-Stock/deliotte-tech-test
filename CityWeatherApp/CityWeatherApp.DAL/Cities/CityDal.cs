using CityWeatherApp.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityWeatherApp.DAL.Cities
{
    public class CityDal : ICityDal
    {
        public async Task<CityRecord> GetById(int id)
        {
            using (CityContext context = new CityContext())
            {
                CityRecord result = await context.Cities.FirstOrDefaultAsync(city => city.Id == id);

                if (result == null)
                {
                    return null;
                }

                return result;
            }
        }

        public async Task AddCity(AddCityRequest city)
        {
            using (CityContext context = new CityContext())
            {
                context.Cities.Add(new CityRecord() {
                    State = city.State,
                    TouristRating = city.TouristRating,
                    Name = city.Name,
                    Country = city.Country,
                    DateEstablished = city.DateEstablished,
                    EstimatedPopulation = city.EstimatedPopulation,
                });

                await context.SaveChangesAsync();
            }
        }

        public async Task<List<CityRecord>> SearchByName(string name)
        {
            using (CityContext context = new CityContext())
            {
                List<CityRecord> cities = await context.Cities.Where(city => city.Name == name).ToListAsync();

                return cities;
            }
        }

        public async Task UpdateById(int id, UpdateCityRequest request)
        {
            using (CityContext context = new CityContext())
            {
                CityRecord toModify = context.Cities.First(city => city.Id == id);
                toModify.TouristRating = request.TouristRating;
                toModify.DateEstablished = request.DateEstablished;
                toModify.EstimatedPopulation = request.EstimatedPopulation;

                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteById(int id)
        {
            using (CityContext context = new CityContext())
            {
                CityRecord toBeDeleted = new CityRecord { Id = id };
                context.Cities.Attach(toBeDeleted);
                context.Cities.Remove(toBeDeleted);
                await context.SaveChangesAsync();
            }
        }
    }

    public interface ICityDal
    {
        public Task<CityRecord> GetById(int id);

        public Task AddCity(AddCityRequest city);

        public Task<List<CityRecord>> SearchByName(string name);

        public Task UpdateById(int id, UpdateCityRequest request);

        public Task DeleteById(int id);
    }
}
