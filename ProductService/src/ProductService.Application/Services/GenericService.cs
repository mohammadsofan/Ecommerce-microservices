using ProductService.Application.Constants;
using ProductService.Application.Interfaces;
using ProductService.Application.Interfaces.IRepository;
using ProductService.Application.Interfaces.IServices;
using ProductService.Application.Wrappers;

namespace ProductService.Application.Services
{
    public class GenericService<TRequest, TResponse, TEntity> : IGenericService<TRequest, TResponse, TEntity>
        where TRequest : class where TResponse : class where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly IAppMapper _mapper;
        private readonly IAppLogger<GenericService<TRequest, TResponse, TEntity>> _logger;

        public GenericService(IGenericRepository<TEntity> repository, IAppMapper mapper, IAppLogger<GenericService<TRequest, TResponse, TEntity>> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResult<TResponse>> AddAsync(TRequest request)
        {
            _logger.LogInformation($"Adding new {typeof(TEntity).Name}.");
            var entity = _mapper.Map<TEntity>(request);
            await _repository.AddAsync(entity);
            _logger.LogInformation($"{typeof(TEntity).Name} created successfully.");
            return ServiceResult<TResponse>.Ok(StatusCodes.CREATED, _mapper.Map<TResponse>(entity), $"{typeof(TEntity).Name} created successfully");
        }

        public async Task<ServiceResult> DeleteAsync(string id)
        {
            _logger.LogInformation($"Deleting {typeof(TEntity).Name} with id {id}.");
            var result = await _repository.DeleteAsync(id);
            if (!result)
            {
                _logger.LogWarning($"{typeof(TEntity).Name} with id {id} not found for deletion.");
                return ServiceResult.Fail(StatusCodes.NOT_FOUND, $"{typeof(TEntity).Name} with id {id} not found");
            }
            _logger.LogInformation($"{typeof(TEntity).Name} with id {id} deleted successfully.");
            return ServiceResult.Ok(StatusCodes.NO_CONTENT, $"{typeof(TEntity).Name} with id {id} deleted successfully");
        }

        public async Task<ServiceResult<IEnumerable<TResponse>>> GetAllAsync()
        {
            _logger.LogInformation($"Retrieving all {typeof(TEntity).Name} records.");
            var entites = await _repository.GetAllAsync();
            var mappedEntites = _mapper.Map<IEnumerable<TResponse>>(entites);
            _logger.LogInformation($"{typeof(TEntity).Name} records retrieved successfully.");
            return ServiceResult<IEnumerable<TResponse>>.Ok(StatusCodes.SUCCESS, mappedEntites, $"{typeof(TEntity).Name} records retrieved successfully");
        }

        public async Task<ServiceResult<TResponse>> GetByIdAsync(string id)
        {
            _logger.LogInformation($"Retrieving {typeof(TEntity).Name} with id {id}.");
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning($"{typeof(TEntity).Name} with id {id} not found.");
                return ServiceResult<TResponse>.Fail(StatusCodes.NOT_FOUND, $"{typeof(TEntity).Name} with id {id} not found");
            }
            _logger.LogInformation($"{typeof(TEntity).Name} with id {id} retrieved successfully.");
            return ServiceResult<TResponse>.Ok(StatusCodes.SUCCESS, _mapper.Map<TResponse>(entity), $"{typeof(TEntity).Name} with id {id} retrieved successfully");
        }

        public async Task<ServiceResult> UpdateAsync(string id, TRequest request)
        {
            _logger.LogInformation($"Updating {typeof(TEntity).Name} with id {id}.");
            var entity = _mapper.Map<TEntity>(request);
            var result = await _repository.UpdateAsync(id, entity);
            if (!result)
            {
                _logger.LogWarning($"{typeof(TEntity).Name} with id {id} not found for update.");
                return ServiceResult.Fail(StatusCodes.NOT_FOUND, $"{typeof(TEntity).Name} with id {id} not found");
            }
            _logger.LogInformation($"{typeof(TEntity).Name} with id {id} updated successfully.");
            return ServiceResult.Ok(StatusCodes.NO_CONTENT, $"{typeof(TEntity).Name} with id {id} updated successfully");
        }
    }
}
