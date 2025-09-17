namespace ProductService.Application.Interfaces.IRepository
{
    public interface IGenericRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(string id);
        Task<T> AddAsync(T entity);
        Task<bool> UpdateAsync(string id, T entity);
        Task<bool> DeleteAsync(string id);
    }
}
