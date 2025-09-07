namespace ProductService.Application.Interfaces.IRepository
{
    public interface IRepository<T>
    {
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
