using CartService.Application.Interfaces;
using CartService.Application.Interfaces.IRepository;
using CartService.Domain.Models;
using CartService.Infrastructure.Data;
using MongoDB.Driver;

namespace CartService.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private new readonly IDbContext<Product> _dbContext;
        private new readonly IAppLogger<Product> _logger;

        public ProductRepository(IDbContext<Product> dbContext, IAppLogger<Product> logger) : base(dbContext, logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> DeleteByOriginalIdAsync(string originalId)
        {
            _logger.LogInformation($"Deleting product with original id: {originalId}");
            var collection = _dbContext.GetCollection();
            var filter = Builders<Product>.Filter.Eq("OriginalId", originalId);
            var result = await collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        public async Task<Product?> GetByOriginalIdAsync(string originalId)
        {
            _logger.LogInformation($"Getting product with original id: {originalId}");
            var collection = _dbContext.GetCollection();
            var filter = Builders<Product>.Filter.Eq("OriginalId", originalId);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
