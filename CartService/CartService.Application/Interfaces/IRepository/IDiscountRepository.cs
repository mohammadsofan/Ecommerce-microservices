using CartService.Domain.Models;

namespace CartService.Application.Interfaces.IRepository
{
    public interface IDiscountRepository : IGenericRepository<Discount>
    {
        Task<bool> DeleteByOriginalIdAsync(string originalId);
        Task<Discount?> GetByOriginalIdAsync(string originalId);
        Task<Discount?> GetByProductId(string productId);
        Task<Discount?> GetByCategoryId(string categoryId);
    }
}
