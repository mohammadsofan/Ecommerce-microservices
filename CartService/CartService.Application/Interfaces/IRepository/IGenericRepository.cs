using System.Collections.Generic;
using System.Threading.Tasks;

namespace CartService.Application.Interfaces.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<bool> UpdateAsync(string id, T entity);
        Task<bool> DeleteAsync(string id);
    }
}
