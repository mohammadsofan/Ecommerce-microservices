using ProductService.Domain.Models;

namespace ProductService.Application.Interfaces.IRepository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
    }
}
