using ProductService.Application.Dtos.Discount;
using ProductService.Domain.Models;

namespace ProductService.Application.Interfaces.IServices
{
    public interface IDiscountService : IGenericService<DiscountRequestDto, DiscountResponseDto, Discount>
    {
    }
}
