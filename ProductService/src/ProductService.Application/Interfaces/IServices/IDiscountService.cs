using ProductService.Application.Dtos.Discount;
using ProductService.Application.Wrappers;
using ProductService.Domain.Models;

namespace ProductService.Application.Interfaces.IServices
{
    public interface IDiscountService : IGenericService<DiscountRequestDto, DiscountResponseDto, Discount>
    {
        Task<ServiceResult<DiscountResponseDto>> GetByProductOrCategoryAsync(string id);
    }
}
