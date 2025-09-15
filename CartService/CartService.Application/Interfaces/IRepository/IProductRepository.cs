using CartService.Domain.Models;

namespace CartService.Application.Interfaces.IRepository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<bool> DeleteByOriginalIdAsync(string originalId);
        Task<Product?> GetByOriginalIdAsync(string originalId);
    }
}
