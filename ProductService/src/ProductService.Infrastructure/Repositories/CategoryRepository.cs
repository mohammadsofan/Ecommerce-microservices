using ProductService.Application.Interfaces.IRepository;
using ProductService.Domain.Models;
using ProductService.Infrastructure.Data;

namespace ProductService.Infrastructure.Repositories
{
    internal class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IDbContext<Category> dbContext) : base(dbContext)
        {
        }
    }
}
