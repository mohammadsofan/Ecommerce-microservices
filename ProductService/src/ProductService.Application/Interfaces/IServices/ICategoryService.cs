using ProductService.Application.Dtos.Category;
using ProductService.Domain.Models;

namespace ProductService.Application.Interfaces.IServices
{
    public interface ICategoryService:IGenericService<CategoryRequestDto,CategoryResponseDto,Category>
    {
    }
}
