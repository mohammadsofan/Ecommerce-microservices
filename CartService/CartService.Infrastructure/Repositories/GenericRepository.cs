using CartService.Application.Interfaces.IRepository;
using CartService.Application.Interfaces;
using CartService.Infrastructure.Data;

namespace CartService.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IDbContext<T> _dbContext;
        protected readonly IAppLogger<T> _logger;

        public GenericRepository(IDbContext<T> dbContext, IAppLogger<T> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            _logger.LogInformation($"Getting {typeof(T).Name} by Id: {id}");
            return await _dbContext.GetDocumentByIdAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            _logger.LogInformation($"Getting all {typeof(T).Name} documents");
            return await _dbContext.GetAllDocumentsAsync();
        }

        public async Task AddAsync(T entity)
        {
            _logger.LogInformation($"Adding new {typeof(T).Name} document");
            await _dbContext.InsertDocumentAsync(entity);
        }

        public async Task UpdateAsync(string id, T entity)
        {
            _logger.LogInformation($"Updating {typeof(T).Name} with Id: {id}");
            await _dbContext.UpdateDocumentAsync(id, entity);
        }

        public async Task DeleteAsync(string id)
        {
            _logger.LogInformation($"Deleting {typeof(T).Name} with Id: {id}");
            await _dbContext.DeleteDocumentAsync(id);
        }
    }
}
