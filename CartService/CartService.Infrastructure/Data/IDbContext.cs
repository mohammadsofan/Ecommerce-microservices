using MongoDB.Driver;

namespace CartService.Infrastructure.Data
{
    public interface IDbContext<T>
    {
        IMongoCollection<T> GetCollection();
        Task<List<T>> GetAllDocumentsAsync();
        Task<T?> GetDocumentByIdAsync(string id);
        public Task<T> InsertDocumentAsync(T payload);
        public Task<bool> UpdateDocumentAsync(string id,T payload);
        public Task<bool> DeleteDocumentAsync(string id);
    }
}