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
        //i want to check the productid first, and return the discount if found, otherwise check the categoryid and return the discount if found
        public async Task<Discount?> GetActiveDiscountByProductOrCategoryAsync(string id)
        {
            var collection = _dbContext.GetCollection();
            var builder = Builders<Discount>.Filter;

            // First, check for an active discount for the product using the provided id
            var productFilter = builder.Eq(x => x.ProductId, id) &
                (builder.Eq(x => x.ExpirationDate, null) | builder.Gt(x => x.ExpirationDate, DateTime.UtcNow));

            var discount = await collection.Find(productFilter).FirstOrDefaultAsync();
            if (discount != null)
            {
                return discount;
            }

            // If not found, check for an active discount for the category using the same id
            var categoryFilter = builder.Eq(x => x.CategoryId, id) &
                (builder.Eq(x => x.ExpirationDate, null) | builder.Gt(x => x.ExpirationDate, DateTime.UtcNow));

            return await collection.Find(categoryFilter).FirstOrDefaultAsync();
        }
    }
}
