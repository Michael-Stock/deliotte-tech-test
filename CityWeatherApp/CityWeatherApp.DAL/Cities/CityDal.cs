using CityWeatherApp.Domain;
using System.Collections.Generic;
using System.Linq;

namespace CityWeatherApp.DAL.Cities
{
    public class CityDal : ICityDal
    {
        public CityRecord GetById(int id)
        {
            using (CityContext context = new CityContext())
            {
                CityRecord result = context.Cities.FirstOrDefault(city => city.Id == id);

                if (result == null)
                {
                    return null;
                }

                return result;
            }
        }

        public void AddCity(AddCityRequest city)
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

                context.SaveChanges();
            }
        }

        public List<CityRecord> SearchByName(string name)
        {
            using (CityContext context = new CityContext())
            {
                List<CityRecord> cities = context.Cities.Where(city => city.Name == name).ToList();

                return cities;
            }
        }

        public void UpdateById(int id, UpdateCityRequest request)
        {
            using (CityContext context = new CityContext())
            {
                CityRecord toModify = context.Cities.First(city => city.Id == id);
                toModify.TouristRating = request.TouristRating;
                toModify.DateEstablished = request.DateEstablished;
                toModify.EstimatedPopulation = request.EstimatedPopulation;

                context.SaveChanges();
            }
        }

        public void DeleteById(int id)
        {
            using (CityContext context = new CityContext())
            {
                CityRecord toBeDeleted = new CityRecord { Id = id };
                context.Cities.Attach(toBeDeleted);
                context.Cities.Remove(toBeDeleted);
                context.SaveChanges();
            }
        }
    }

    public interface ICityDal
    {
        public CityRecord GetById(int id);

        public void AddCity(AddCityRequest city);

        public List<CityRecord> SearchByName(string name);

        public void UpdateById(int id, UpdateCityRequest request);

        public void DeleteById(int id);
    }
}
