using MongoDB.Driver;
using ProductService.Application.Interfaces.IRepository;
using ProductService.Domain.Models;
using ProductService.Infrastructure.Data;

namespace ProductService.Infrastructure.Repositories
{
    public class DiscountRepository : GenericRepository<Discount>, IDiscountRepository
    {
        private readonly IDbContext<Discount> _dbContext;

        public DiscountRepository(IDbContext<Discount> dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> HasActiveDiscountForProduct(string productId, string? excludeDiscountId = null)
        {
            var collection = _dbContext.GetCollection();
            var builder = Builders<Discount>.Filter;
            var filter = builder.Eq(x => x.ProductId, productId);

            if (excludeDiscountId != null)
            {
                filter &= builder.Ne(x => x.Id, excludeDiscountId);
            }

            // Check for active discounts (not expired or expiration date is in the future)
            filter &= builder.Or(
                builder.Eq(x => x.ExpirationDate, null),
                builder.Gt(x => x.ExpirationDate, DateTime.UtcNow)
            );

            return await collection.Find(filter).AnyAsync();
        }

        public async Task<bool> HasActiveDiscountForCategory(string categoryId, string? excludeDiscountId = null)
        {
            var collection = _dbContext.GetCollection();
            var builder = Builders<Discount>.Filter;
            var filter = builder.Eq(x => x.CategoryId, categoryId);

            if (excludeDiscountId != null)
            {
                filter &= builder.Ne(x => x.Id, excludeDiscountId);
            }

            // Check for active discounts (not expired or expiration date is in the future)
            filter &= builder.Or(
                builder.Eq(x => x.ExpirationDate, null),
                builder.Gt(x => x.ExpirationDate, DateTime.UtcNow)
            );

            return await collection.Find(filter).AnyAsync();
        }
    }
}
