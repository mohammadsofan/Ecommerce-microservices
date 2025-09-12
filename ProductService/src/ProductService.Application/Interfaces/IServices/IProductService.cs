using ProductService.Application.Dtos.Product;
using ProductService.Domain.Models;

namespace ProductService.Application.Interfaces.IServices
{
    public interface IProductService : IGenericService<ProductRequestDto, ProductResponseDto, Product>
    {
    }
}
