using ProductService.Application.Interfaces.IRepository;
using ProductService.Infrastructure.Data;

namespace ProductService.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T>
    {
        private readonly IDbContext<T> dbContext;

        public GenericRepository(IDbContext<T> dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<T> AddAsync(T entity)
        {
            return await dbContext.InsertDocumentAsync(entity);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await dbContext.DeleteDocumentAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbContext.GetAllDocumentsAsync();
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            return await dbContext.GetDocumentByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            return await dbContext.UpdateDocumentAsync(entity);
        }
    }
}
