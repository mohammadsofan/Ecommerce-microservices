using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace IdentityService.Application.IRepository
{
    public interface IGenericRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAsync(FilterDefinition<T> filter);
    }
}