using ProductService.Application.Wrappers;

namespace ProductService.Application.Interfaces.IServices
{
    public interface IGenericService<TRequest,TResponse,TEntity>
    {
        Task<ServiceResult<IEnumerable<TResponse>>> GetAllAsync();
        Task<ServiceResult<TResponse>> GetByIdAsync(string id);
        Task<ServiceResult<TResponse>> AddAsync(TRequest request);
        Task<ServiceResult> UpdateAsync(string id,TRequest request);
        Task<ServiceResult> DeleteAsync(string id);
    }
}
