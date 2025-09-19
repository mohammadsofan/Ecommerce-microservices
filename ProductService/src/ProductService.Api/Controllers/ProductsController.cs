using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Constants;
using ProductService.Application.Dtos.Product;
using ProductService.Application.Interfaces.IServices;
using Shared.Events;
namespace ProductService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, IPublishEndpoint publishEndpoint, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            _logger.LogInformation("Getting all products.");
            var result = await _productService.GetAllAsync();
            _logger.LogInformation("Retrieved all products.");
            return Ok(result);
        }
        [HttpGet("{id}", Name = "GetProductByIdAsync")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductByIdAsync(string id)
        {
            _logger.LogInformation($"Getting product by id: {id}");
            var result = await _productService.GetByIdAsync(id);
            if (!result.Success && result.StatusCode == Application.Constants.StatusCodes.NOT_FOUND)
            {
                _logger.LogWarning($"Product with id {id} not found.");
                return NotFound(result);
            }
            else if (!result.Success && result.StatusCode == Application.Constants.StatusCodes.BAD_REQUEST)
            {
                _logger.LogWarning($"Product retriving failed: bad request (id: {id}).");
                return BadRequest(result);
            }
            _logger.LogInformation($"Product with id {id} retrieved successfully.");
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles = ApplicationRoles.ADMIN)]
        public async Task<IActionResult> CreateProductAsync([FromBody] ProductRequestDto requestDto)
        {
            _logger.LogInformation("Creating a new product.");
            var result = await _productService.AddAsync(requestDto);
            if (!result.Success && result.StatusCode == Application.Constants.StatusCodes.NOT_FOUND)
            {
                _logger.LogWarning("Product creation failed: related entity not found.");
                return NotFound(result);
            }
            else if (!result.Success && result.StatusCode == Application.Constants.StatusCodes.BAD_REQUEST)
            {
                _logger.LogWarning("Product creation failed: bad request.");
                return BadRequest(result);
            }
            _logger.LogInformation($"Product created successfully with id: {result.Data?.Id}");
            if (result.Data is not null)
            {
                _logger.LogInformation($"Publishing ProductCreatedEvent for product id: {result.Data.Id}");
                await _publishEndpoint.Publish(new ProductCreatedEvent()
                {
                    Id = result.Data.Id,
                    CategoryId = result.Data.CategoryId,
                    Price = result.Data.Price,
                    Stock = result.Data.Stock
                });
                _logger.LogInformation($"Published ProductCreatedEvent for product id: {result.Data.Id}");
            }
            return CreatedAtRoute(nameof(GetProductByIdAsync), new { id = result.Data?.Id }, result);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = ApplicationRoles.ADMIN)]
        public async Task<IActionResult> UpdateProductAsync([FromRoute] string id, [FromBody] ProductRequestDto requestDto)
        {
            _logger.LogInformation($"Updating product with id: {id}");
            var result = await _productService.UpdateAsync(id, requestDto);
            if (!result.Success && result.StatusCode == Application.Constants.StatusCodes.NOT_FOUND)
            {
                _logger.LogWarning($"Product update failed: product or related entity not found (id: {id}).");
                return NotFound(result);
            }
            else if (!result.Success && result.StatusCode == Application.Constants.StatusCodes.BAD_REQUEST)
            {
                _logger.LogWarning($"Product update failed: bad request (id: {id}).");
                return BadRequest(result);
            }
            if (result.Success)
            {
                _logger.LogInformation($"Publishing ProductUpdatedEvent for product id: {id}");
                var productResult = await _productService.GetByIdAsync(id);
                await _publishEndpoint.Publish(new ProductCreatedEvent()
                {
                    Id = productResult.Data!.Id,
                    CategoryId = productResult.Data!.CategoryId,
                    Price = productResult.Data!.Price,
                    Stock = productResult.Data!.Stock
                });
                _logger.LogInformation($"Published ProductUpdatedEvent for product id: {id}");
            }
            _logger.LogInformation($"Product with id {id} updated successfully.");
            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = ApplicationRoles.ADMIN)]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] string id)
        {
            _logger.LogInformation($"Deleting product with id: {id}");
            var result = await _productService.DeleteAsync(id);
            if (!result.Success && result.StatusCode == Application.Constants.StatusCodes.NOT_FOUND)
            {
                _logger.LogWarning($"Product delete failed: product not found (id: {id}).");
                return NotFound(result);
            }
            else if (!result.Success && result.StatusCode == Application.Constants.StatusCodes.BAD_REQUEST)
            {
                _logger.LogWarning($"Product delete failed: bad request (id: {id}).");
                return BadRequest(result);
            }
            _logger.LogInformation($"Product with id {id} deleted successfully.");
            if (result.Success)
            {
                _logger.LogInformation($"Publishing ProductDeletedEvent for product id: {id}");
                await _publishEndpoint.Publish(new ProductDeletedEvent() { Id = id });
                _logger.LogInformation($"Published ProductDeletedEvent for product id: {id}");
            }
            return NoContent();
        }

    }
}
