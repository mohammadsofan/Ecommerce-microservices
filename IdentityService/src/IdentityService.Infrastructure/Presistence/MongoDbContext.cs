using System;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using IdentityService.Domain.Entities; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Infrastructure.Presistence
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var mongoSettings = configuration.GetSection("MongoSettings");
            var mongoClient = new MongoClient(mongoSettings.GetValue<string>("ConnectionString"));
            _database = mongoClient.GetDatabase(mongoSettings.GetValue<string>("DatabaseName"));
        }
        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");

        // Generic method to get a collection by type name
        public IMongoCollection<T> GetCollection<T>()
        {
            var collectionName = typeof(T).Name + "s";
            return _database.GetCollection<T>(collectionName);
        }
    }
}