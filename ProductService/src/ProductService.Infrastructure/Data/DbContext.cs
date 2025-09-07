using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ProductService.Infrastructure.Data
{
    internal class DbContext<T> : IDbContext<T>
    {

        private readonly IMongoDatabase _db;
        public DbContext(IMongoClient mongoClient, IOptions<MongoDbSettings> options)
        {
            _db = mongoClient.GetDatabase(options.Value.DatabaseName);
        }
        public IMongoCollection<T> GetCollection()
        {
            return _db.GetCollection<T>(typeof(T).Name);
        }
        public async Task<List<T>> GetAllDocumentsAsync()
        {
            return await GetCollection().Find(_ => true).ToListAsync();
        }
        public async Task<T?> GetDocumentByIdAsync(string id)
        {
            return await GetCollection().Find(Builders<T>.Filter.Eq("Id", id)).FirstOrDefaultAsync();
        }
        public async Task<T> InsertDocumentAsync(T payload)
        {
            await GetCollection().InsertOneAsync(payload);
            return payload;
        }
        public async Task<bool> UpdateDocumentAsync(T payload)
        {
            if (payload == null) return false;
            var id = payload.GetType().GetProperty("Id")?.GetValue(payload)?.ToString();
            if (string.IsNullOrEmpty(id)) return false;
            var document = await GetDocumentByIdAsync(id);
            if (document == null) return false;
            await GetCollection().ReplaceOneAsync(Builders<T>.Filter.Eq("Id", id), payload);
            return true;
        }
        public async Task<bool> DeleteDocumentAsync(string id)
        {
            var document = await GetDocumentByIdAsync(id);
            if (document == null) return false;
            await GetCollection().DeleteOneAsync(Builders<T>.Filter.Eq("Id", id));
            return true;
        }

    }
}
