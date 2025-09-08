using ProductService.Application.Constants;
using ProductService.Application.Interfaces;
using ProductService.Application.Interfaces.IRepository;
using ProductService.Application.Interfaces.IServices;
using ProductService.Application.Wrappers;

namespace ProductService.Application.Services
{
    public class GenericService<TRequest,TResponse,TEntity> : IGenericService<TRequest,TResponse,TEntity>
        where TRequest : class where TResponse : class where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly IAppMapper _mapper;

        public GenericService(IGenericRepository<TEntity> repository,IAppMapper mapper) {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResult<TResponse>> AddAsync(TRequest request)
        {
            var entity = _mapper.Map<TEntity>(request);
            await _repository.AddAsync(entity);
            return ServiceResult<TResponse>.Ok(StatusCodes.CREATED,_mapper.Map<TResponse>(entity),$"{typeof(TEntity).Name} created successfully");
        }

        public async Task<ServiceResult> DeleteAsync(string id)
        {
            var result = await _repository.DeleteAsync(id);
            if(!result)
                return ServiceResult.Fail(StatusCodes.NOT_FOUND,$"{typeof(TEntity).Name} with id {id} not found");
            return ServiceResult.Ok(StatusCodes.NO_CONTENT,$"{typeof(TEntity).Name} with id {id} deleted successfully");
        }

        public async Task<ServiceResult<IEnumerable<TResponse>>> GetAllAsync()
        {
            var entites = await _repository.GetAllAsync();
            var mappedEntites = _mapper.Map<IEnumerable<TResponse>>(entites);
            return ServiceResult<IEnumerable<TResponse>>.Ok(StatusCodes.SUCCESS,mappedEntites,$"{typeof(TEntity).Name} records retrieved successfully");
        }

        public async Task<ServiceResult<TResponse>> GetByIdAsync(string id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if(entity == null)
                return ServiceResult<TResponse>.Fail(StatusCodes.NOT_FOUND,$"{typeof(TEntity).Name} with id {id} not found");
            return ServiceResult<TResponse>.Ok(StatusCodes.SUCCESS,_mapper.Map<TResponse>(entity),$"{typeof(TEntity).Name} with id {id} retrieved successfully");
        }

        public async Task<ServiceResult> UpdateAsync(string id,TRequest request)
        {
            var entity = _mapper.Map<TEntity>(request);
            var result = await _repository.UpdateAsync(id,entity);
            if(!result)
                return ServiceResult.Fail(StatusCodes.NOT_FOUND,$"{typeof(TEntity).Name} with id {id} not found");
            return ServiceResult.Ok(StatusCodes.NO_CONTENT,$"{typeof(TEntity).Name} with id {id} updated successfully");

        }
    }
}
