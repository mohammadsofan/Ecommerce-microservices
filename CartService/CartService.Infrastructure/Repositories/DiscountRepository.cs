using CartService.Application.Interfaces;
using CartService.Application.Interfaces.IRepository;
using CartService.Domain.Models;
using CartService.Infrastructure.Data;
using MongoDB.Driver;

namespace CartService.Infrastructure.Repositories
{
    public class DiscountRepository : GenericRepository<Discount>, IDiscountRepository
    {
        public DiscountRepository(IDbContext<Discount> dbContext, IAppLogger<Discount> logger) : base(dbContext, logger)
        {

        }
        public async Task<bool> DeleteByOriginalIdAsync(string originalId)
        {
            _logger.LogInformation($"Deleting discount with original id: {originalId}");
            var collection = _dbContext.GetCollection();
            var filter = Builders<Discount>.Filter.Eq("OriginalId", originalId);
            var result = await collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        public async Task<Discount?> GetByOriginalIdAsync(string originalId)
        {
            _logger.LogInformation($"Getting discount with original id: {originalId}");
            var collection = _dbContext.GetCollection();
            var filter = Builders<Discount>.Filter.Eq("OriginalId", originalId);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Discount?> GetByProductId(string productId)
        {
            _logger.LogInformation($"Getting discount with product id: {productId}");
            var collection = _dbContext.GetCollection();
            var filter = Builders<Discount>.Filter.Eq("ProductId",productId);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<Discount?> GetByCategoryId(string categoryId)
        {
            _logger.LogInformation($"Getting discount with category id: {categoryId}");
            var collection = _dbContext.GetCollection();
            var filter = Builders<Discount>.Filter.Eq("CategoryId", categoryId);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
