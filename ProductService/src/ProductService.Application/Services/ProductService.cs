using ProductService.Application.Dtos.Product;
using ProductService.Application.Interfaces;
using ProductService.Application.Interfaces.IRepository;
using ProductService.Application.Interfaces.IServices;
using ProductService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Services
{
    public class ProductService : GenericService<ProductRequestDto, ProductResponseDto, Product>, IProductService
    {
        public ProductService(IGenericRepository<Product> repository, IAppMapper mapper, IAppLogger<GenericService<ProductRequestDto, ProductResponseDto, Product>> logger)
            : base(repository, mapper, logger)
        {
        }
    }
}
