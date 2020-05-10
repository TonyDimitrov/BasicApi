namespace CarsApi.Data.Seed
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CarsApi.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class CarsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            if (!(await dbContext.Cars.AnyAsync()))
            {
                await SeedAdminUser(dbContext);
            }
        }

        private static async Task SeedAdminUser(ApplicationDbContext dbContext)
        {
            var cars = new List<Car>
           {
               new Car
               {
                   CreatedOn = DateTime.UtcNow,
                   Brand = "Ford",
                   YearOfProduction = new DateTime(2000, 10, 05),
               },
               new Car
               {
                   CreatedOn = DateTime.UtcNow,
                   Brand = "Opel",
                   YearOfProduction = new DateTime(2005, 10, 05),
               },
               new Car
               {
                   CreatedOn = DateTime.UtcNow,
                   Brand = "Renaugh",
                   YearOfProduction = new DateTime(2006, 10, 05),
               },
               new Car
               {
                   CreatedOn = DateTime.UtcNow,
                   Brand = "Honda",
                   YearOfProduction = new DateTime(2015, 10, 05),
               },
               new Car
               {
                   CreatedOn = DateTime.UtcNow,
                   Brand = "Lada",
                   YearOfProduction = new DateTime(1999, 10, 05),
               },
               new Car
               {
                   CreatedOn = DateTime.UtcNow,
                   Brand = "BMW",
                   YearOfProduction = new DateTime(2019, 10, 05),
               },
               new Car
               {
                   CreatedOn = DateTime.UtcNow,
                   Brand = "Audi",
                   YearOfProduction = new DateTime(2011, 10, 05),
               },
           };

            foreach (var car in cars)
            {
                dbContext.Add(car);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
