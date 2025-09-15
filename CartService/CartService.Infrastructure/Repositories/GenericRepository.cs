using CartService.Application.Interfaces.IRepository;
using CartService.Infrastructure.Data;

namespace CartService.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IDbContext<T> _dbContext;

        public GenericRepository(IDbContext<T> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            return await _dbContext.GetDocumentByIdAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.GetAllDocumentsAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbContext.InsertDocumentAsync(entity);
        }

        public async Task UpdateAsync(string id, T entity)
        {
            await _dbContext.UpdateDocumentAsync(id, entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _dbContext.DeleteDocumentAsync(id);
        }
    }
}
