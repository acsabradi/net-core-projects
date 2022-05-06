using Microsoft.EntityFrameworkCore;
using OdeToFood.Core;
using System.Collections.Generic;
using System.Linq;

namespace OdeToFood.Data
{
    public class SqlRestaurantData : IRestaurantData
    {
        private readonly OdeToFoodDbContext db;

        public SqlRestaurantData(OdeToFoodDbContext db)
        {
            this.db = db;
        }

        public Restaurant Add(Restaurant newRestaurant)
        {
            //A DbContext.Add() metódussal hozzáadunk egy új entity-t a DbSet-hez
            db.Add(newRestaurant);
            return newRestaurant;
        }

        public int Commit()
        {
            //Nem történik változás az adatbázisban, amíg a SaveChanges() nem fut le
            return db.SaveChanges();
        }

        public Restaurant Delete(int id)
        {
            //Megkeressük ID alapján, ha megvan, akkor töröljük a DbSet-ből
            var restaurant = GetById(id);
            if (restaurant != null)
            {
                db.Restaurants.Remove(restaurant);
            }
            return restaurant;
        }

        public Restaurant GetById(int id)
        {
            //A Find() primary key alapján keres entity-t
            return db.Restaurants.Find(id);
        }

        public int GetCountOfRestaurants()
        {
            return db.Restaurants.Count();
        }

        public IEnumerable<Restaurant> GetRestaurantsByName(string name)
        {
            //u.a. mint az InMemory implementációban
            var query = from r in db.Restaurants
                        where r.Name.StartsWith(name) || string.IsNullOrEmpty(name)
                        orderby r.Name
                        select r;
            return query;
        }

        public Restaurant Update(Restaurant updatedRestaurant)
        {
            /*
             * Az Attach() és a State lementik, hogy ez az entity megváltozott
             * és a Commit() során az adatbázisnak frissíteni kell a property-jeit
            */
            var entity = db.Restaurants.Attach(updatedRestaurant);
            entity.State = EntityState.Modified;
            return updatedRestaurant;
        }
    }
}
