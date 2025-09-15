using System.Collections.Generic;
using System.Threading.Tasks;
using CartService.Domain.Models;

namespace CartService.Application.Interfaces.IRepository
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart?> GetCartByUserIdAsync(string userId);
        Task AddItemToCartAsync(string cartId, CartItem item);
        Task RemoveItemFromCartAsync(string cartId, string productId);
        Task UpdateItemQuantityAsync(string cartId, string productId, int quantity);
        Task ClearCartAsync(string cartId);
    }
}
