using ProductService.Application.Dtos.Product;
using ProductService.Application.Interfaces;
using ProductService.Application.Interfaces.IRepository;
using ProductService.Application.Interfaces.IServices;
using ProductService.Application.Wrappers;
using ProductService.Domain.Models;

namespace ProductService.Application.Services
{
    public class ProductService : GenericService<ProductRequestDto, ProductResponseDto, Product>, IProductService
    {
        private readonly ICategoryService _categoryService;

        public ProductService(IGenericRepository<Product> repository, ICategoryService categoryService, IAppMapper mapper, IAppLogger<GenericService<ProductRequestDto, ProductResponseDto, Product>> logger)
            : base(repository, mapper, logger)
        {
            _categoryService = categoryService;
        }
        public override async Task<ServiceResult<ProductResponseDto>> AddAsync(ProductRequestDto request)
        {
            var result = await _categoryService.GetByIdAsync(request.CategoryId);
            if (!result.Success)
            {
                return ServiceResult<ProductResponseDto>.Fail(result.StatusCode,
                    result.Message,
                    result.Errors);
            }
            return await base.AddAsync(request);
        }
        public override async Task<ServiceResult> UpdateAsync(string id, ProductRequestDto request)
        {
            var result = await _categoryService.GetByIdAsync(request.CategoryId);
            if (!result.Success)
            {
                return ServiceResult.Fail(result.StatusCode,
                    result.Message,
                    result.Errors);
            }
            return await base.UpdateAsync(id, request);
        }
    }
}
