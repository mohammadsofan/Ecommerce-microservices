using ProductService.Domain.Models;

namespace ProductService.Application.Interfaces.IRepository
{
    public interface IDiscountRepository : IGenericRepository<Discount>
    {
        Task<bool> HasActiveDiscountForProduct(string productId, string? excludeDiscountId = null);
        Task<bool> HasActiveDiscountForCategory(string categoryId, string? excludeDiscountId = null);
        Task<Discount?> GetActiveDiscountByProductOrCategoryAsync(string id);
    }
}
