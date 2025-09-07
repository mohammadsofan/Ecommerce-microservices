using MongoDB.Driver;

namespace ProductService.Infrastructure.Data
{
    public interface IDbContext
    {
        IMongoCollection<T> GetCollection<T>();
        Task<List<T>> GetAllDocumentsAsync<T>();
        Task<T?> GetDocumentByIdAsync<T>(string id);
        public Task<T> InsertDocument<T>(T payload);
        public Task<bool> UpdateDocument<T>(T payload);
        public Task<bool> DeleteDocument<T>(string id);
    }
}