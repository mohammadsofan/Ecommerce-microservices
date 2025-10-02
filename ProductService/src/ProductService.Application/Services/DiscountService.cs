using ProductService.Application.Constants;
using ProductService.Application.Dtos.Discount;
using ProductService.Application.Interfaces;
using ProductService.Application.Interfaces.IRepository;
using ProductService.Application.Interfaces.IServices;
using ProductService.Application.Wrappers;
using ProductService.Domain.Enums;
using ProductService.Domain.Models;

namespace ProductService.Application.Services
{
    public class DiscountService : GenericService<DiscountRequestDto, DiscountResponseDto, Discount>, IDiscountService
    {
        private readonly IAppMapper _mapper;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IDiscountRepository _discountRepository;

        public DiscountService(IGenericRepository<Discount> repository,
            IAppMapper mapper,
            IAppLogger<GenericService<DiscountRequestDto, DiscountResponseDto, Discount>> logger,
            ICategoryService categoryService,
            IProductService productService,
            IDiscountRepository discountRepository) : base(repository, mapper, logger)
        {
            _mapper = mapper;
            _categoryService = categoryService;
            _productService = productService;
            _discountRepository = discountRepository;
        }

        private async Task<ServiceResult> ValidateDiscount(DiscountRequestDto request, string? excludeDiscountId = null)
        {
            if (request.CategoryId == null && request.ProductId == null)
            {
                return ServiceResult.Fail(StatusCodes.BAD_REQUEST,
                    "Either CategoryId or ProductId must be provided.",
                    new List<Error> { new Error { Field = "CategoryId/ProductId", Message = "Either CategoryId or ProductId must be provided." } });
            }
            if (request.ProductId != null && request.CategoryId != null)
            {
                return ServiceResult.Fail(StatusCodes.BAD_REQUEST,
                    "Only one of CategoryId or ProductId should be provided.",
                    new List<Error> { new Error { Field = "CategoryId/ProductId", Message = "Only one of CategoryId or ProductId should be provided." } });
            }
            // Check if product exists and has no active discount
            if (request.ProductId != null)
            {
                var productResult = await _productService.GetByIdAsync(request.ProductId);
                if (!productResult.Success)
                {
                    return ServiceResult.Fail(productResult.StatusCode,
                        productResult.Message,
                        productResult.Errors);
                }

                if (await _discountRepository.HasActiveDiscountForProduct(request.ProductId, excludeDiscountId))
                {
                    return ServiceResult.Fail(StatusCodes.CONFLICT,
                        "Product already has an active discount.",
                        new List<Error> { new Error { Field = "ProductId", Message = "Product already has an active discount." } });
                }
            }
            // Check if category exists and has no active discount
            else if (request.CategoryId != null)
            {
                var categoryResult = await _categoryService.GetByIdAsync(request.CategoryId);
                if (!categoryResult.Success)
                {
                    return ServiceResult.Fail(categoryResult.StatusCode,
                        categoryResult.Message,
                        categoryResult.Errors);
                }

                if (await _discountRepository.HasActiveDiscountForCategory(request.CategoryId, excludeDiscountId))
                {
                    return ServiceResult.Fail(StatusCodes.CONFLICT,
                        "Category already has an active discount.",
                        new List<Error> { new Error { Field = "CategoryId", Message = "Category already has an active discount." } });
                }
            }

            if (request.Amount < 0)
            {
                return ServiceResult.Fail(StatusCodes.BAD_REQUEST,
                    "Amount must be a non-negative value.",
                    new List<Error> { new Error { Field = "Amount", Message = "Amount must be a non-negative value." } });
            }

            if (request.DiscountType == DiscountType.Percentage && request.Amount > 100)
            {
                return ServiceResult.Fail(StatusCodes.BAD_REQUEST,
                    "Percentage discount cannot exceed 100%.",
                    new List<Error> { new Error { Field = "Amount", Message = "Percentage discount cannot exceed 100%." } });
            }

            if (request.ExpirationDate.HasValue && request.ExpirationDate.Value <= DateTime.UtcNow)
            {
                return ServiceResult.Fail(StatusCodes.BAD_REQUEST,
                    "Expiration date must be in the future.",
                    new List<Error> { new Error { Field = "ExpirationDate", Message = "Expiration date must be in the future." } });
            }

            return ServiceResult.Ok(StatusCodes.SUCCESS);
        }

        public override async Task<ServiceResult<DiscountResponseDto>> AddAsync(DiscountRequestDto request)
        {
            var validationResult = await ValidateDiscount(request);
            if (!validationResult.Success)
            {
                return ServiceResult<DiscountResponseDto>.Fail(
                    validationResult.StatusCode,
                    validationResult.Message,
                    validationResult.Errors);
            }
            return await base.AddAsync(request);
        }

        public override async Task<ServiceResult> UpdateAsync(string id, DiscountRequestDto request)
        {
            var validationResult = await ValidateDiscount(request, id);
            if (!validationResult.Success)
            {
                return ServiceResult.Fail(
                    validationResult.StatusCode,
                    validationResult.Message,
                    validationResult.Errors);
            }
            return await base.UpdateAsync(id, request);
        }
        public async Task<ServiceResult<DiscountResponseDto>> GetByProductOrCategoryAsync(string id)
        {

            var discount = await _discountRepository.GetActiveDiscountByProductOrCategoryAsync(id);
            if (discount == null)
            {
                return ServiceResult<DiscountResponseDto>.Fail(StatusCodes.NOT_FOUND, "No active discount found for the given id.");
            }

            return ServiceResult<DiscountResponseDto>.Ok(StatusCodes.SUCCESS, _mapper.Map<DiscountResponseDto>(discount), "Active discount retrieved successfully.");
        }
    }
}
