using CartService.Application.Interfaces;
using CartService.Application.Interfaces.IRepository;
using CartService.Domain.Models;
using CartService.Infrastructure.Data;
using MongoDB.Driver;
namespace CartService.Infrastructure.Repositories
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        private new readonly IDbContext<Cart> _dbContext;
        private new readonly IAppLogger<Cart> _logger;

        public CartRepository(IDbContext<Cart> dbContext, IAppLogger<Cart> logger) : base(dbContext, logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Cart?> GetCartByUserIdAsync(string userId)
        {
            _logger.LogInformation($"Getting cart for userId: {userId}");
            var collection = _dbContext.GetCollection();
            var filter = Builders<Cart>.Filter.Eq(c => c.UserId, userId);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task AddItemToCartAsync(string cartId, CartItem item)
        {
            _logger.LogInformation($"Adding item to cartId: {cartId}, productId: {item.ProductId}, quantity: {item.Quantity}");
            var collection = _dbContext.GetCollection();
            var filter = Builders<Cart>.Filter.Eq(c => c.Id, cartId);
            var update = Builders<Cart>.Update.Push(c => c.Items, item);
            await collection.UpdateOneAsync(filter, update);
        }

        public async Task RemoveItemFromCartAsync(string cartId, string productId)
        {
            _logger.LogInformation($"Removing item from cartId: {cartId}, productId: {productId}");
            var collection = _dbContext.GetCollection();
            var filter = Builders<Cart>.Filter.Eq(c => c.Id, cartId);
            var update = Builders<Cart>.Update.PullFilter(c => c.Items, i => i.ProductId == productId);
            await collection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateItemQuantityAsync(string cartId, string productId, int quantity)
        {
            _logger.LogInformation($"Updating item quantity in cartId: {cartId}, productId: {productId}, quantity: {quantity}");
            var collection = _dbContext.GetCollection();
            var filter = Builders<Cart>.Filter.And(
                Builders<Cart>.Filter.Eq(c => c.Id, cartId),
                Builders<Cart>.Filter.ElemMatch(c => c.Items, i => i.ProductId == productId)
            );
            var update = Builders<Cart>.Update.Set("Items.$.Quantity", quantity);
            await collection.UpdateOneAsync(filter, update);
        }

        public async Task ClearCartAsync(string cartId)
        {
            _logger.LogInformation($"Clearing cartId: {cartId}");
            var collection = _dbContext.GetCollection();
            var filter = Builders<Cart>.Filter.Eq(c => c.Id, cartId);
            var update = Builders<Cart>.Update.Set(c => c.Items, new List<CartItem>());
            await collection.UpdateOneAsync(filter, update);
        }
    }
}
