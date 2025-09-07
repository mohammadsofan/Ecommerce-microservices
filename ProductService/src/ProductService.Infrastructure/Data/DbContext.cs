using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProductService.Application.Interfaces;

namespace ProductService.Infrastructure.Data
{
    internal class DbContext<T> : IDbContext<T>
    {

        private readonly IMongoDatabase _db;
        private readonly IAppLogger<T> _logger;

        public DbContext(IMongoClient mongoClient, IOptions<MongoDbSettings> options, IAppLogger<T> logger)
        {
            _logger = logger;
            _logger.LogInformation($"Connecting to MongoDB database: {options.Value.DatabaseName}");
            _db = mongoClient.GetDatabase(options.Value.DatabaseName);
            _logger.LogInformation("MongoDB connection established.");
        }
        public IMongoCollection<T> GetCollection()
        {
            var collectionName = typeof(T).Name;
            _logger.LogDebug($"Accessing collection: {collectionName}");
            return _db.GetCollection<T>(collectionName);
        }
        public async Task<List<T>> GetAllDocumentsAsync()
        {
            _logger.LogInformation($"Retrieving all documents from collection: {typeof(T).Name}");
            return await GetCollection().Find(_ => true).ToListAsync();
        }
        public async Task<T?> GetDocumentByIdAsync(string id)
        {
            _logger.LogInformation($"Retrieving document by Id: {id} from collection: {typeof(T).Name}");
            return await GetCollection().Find(Builders<T>.Filter.Eq("Id", id)).FirstOrDefaultAsync();
        }
        public async Task<T> InsertDocumentAsync(T payload)
        {
            _logger.LogInformation($"Inserting new document into collection: {typeof(T).Name}");
            await GetCollection().InsertOneAsync(payload);
            return payload;
        }
        public async Task<bool> UpdateDocumentAsync(T payload)
        {
            if (payload == null)
            {
                _logger.LogWarning("Update attempted with null payload.");
                return false;
            }
            var id = payload.GetType().GetProperty("Id")?.GetValue(payload)?.ToString();
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("Update attempted with payload missing Id property.");
                return false;
            }
            _logger.LogInformation($"Updating document with Id: {id} in collection: {typeof(T).Name}");
            var document = await GetDocumentByIdAsync(id);
            if (document == null)
            {
                _logger.LogWarning($"No document found with Id: {id} to update in collection: {typeof(T).Name}");
                return false;
            }
            await GetCollection().ReplaceOneAsync(Builders<T>.Filter.Eq("Id", id), payload);
            return true;
        }
        public async Task<bool> DeleteDocumentAsync(string id)
        {
            _logger.LogInformation($"Deleting document with Id: {id} from collection: {typeof(T).Name}");
            var document = await GetDocumentByIdAsync(id);
            if (document == null)
            {
                _logger.LogWarning($"No document found with Id: {id} to delete in collection: {typeof(T).Name}");
                return false;
            }
            await GetCollection().DeleteOneAsync(Builders<T>.Filter.Eq("Id", id));
            return true;
        }

    }
}
