using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoiceService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace InvoiceService.Infrastructure.Presistence
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
        public IMongoCollection<Invoice> Invoices => _database.GetCollection<Invoice>("Invoices");
       
        public IMongoCollection<T> GetCollection<T>()
        {
            var collectionName = typeof(T).Name + "s";
            return _database.GetCollection<T>(collectionName);
        }
    }
}