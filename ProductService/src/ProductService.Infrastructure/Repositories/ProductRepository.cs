using ProductService.Application.Interfaces.IRepository;
using ProductService.Domain.Models;
using ProductService.Infrastructure.Data;

namespace ProductService.Infrastructure.Repositories
{
    internal class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(IDbContext<Product> dbContext) : base(dbContext)
        {
        }
    }
}
