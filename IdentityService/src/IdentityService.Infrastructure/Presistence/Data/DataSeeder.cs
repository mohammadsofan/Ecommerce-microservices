using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace IdentityService.Infrastructure.Presistence.Data
{
    public class DataSeeder
    {
        public static async Task SeedAsync(MongoDbContext context)
        {
            if (!context.Users.AsQueryable().Any())
            {
                var addressGenerator = new AddressDataGenerator();
                var addresses = addressGenerator.GenerateAddresses();
                var userGenerator = new UserDataGenerator(addresses);
                var users = userGenerator.GenerateUsers();
                await context.Users.InsertManyAsync(users);
            }

        }
    }
}