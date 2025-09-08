using ProductService.Application.Dtos.Category;
using ProductService.Application.Interfaces;
using ProductService.Application.Interfaces.IRepository;
using ProductService.Application.Interfaces.IServices;
using ProductService.Domain.Models;

namespace ProductService.Application.Services
{
    public class CategoryService : GenericService<CategoryRequestDto, CategoryResponseDto, Category>, ICategoryService
    {
        public CategoryService(IGenericRepository<Category> repository, IAppMapper mapper) : base(repository, mapper)
        {
        }
    }
}
