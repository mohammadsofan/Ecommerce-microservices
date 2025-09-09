using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Dtos.Category;
using ProductService.Application.Interfaces.IServices;

namespace ProductService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            _logger.LogInformation("Getting all categories.");
            var result = await _categoryService.GetAllAsync();
            _logger.LogInformation("Retrieved all categories.");
            return Ok(result);
        }
        [HttpGet("{id}", Name = "GetCategoryByIdAsync")]
        public async Task<IActionResult> GetCategoryByIdAsync([FromRoute] string id)
        {
            _logger.LogInformation($"Getting category by id: {id}");
            var result = await _categoryService.GetByIdAsync(id);
            if (result.Data == null)
            {
                _logger.LogWarning($"Category with id {id} not found.");
                return NotFound(result);
            }
            _logger.LogInformation($"Category with id {id} retrieved successfully.");
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CategoryRequestDto requestDto)
        {
            _logger.LogInformation("Creating a new category.");
            var result = await _categoryService.AddAsync(requestDto);
            _logger.LogInformation($"Category created successfully with id: {result.Data?.Id}");
            return CreatedAtRoute(nameof(GetCategoryByIdAsync), new { id = result.Data?.Id }, result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryAsync([FromRoute] string id)
        {
            _logger.LogInformation($"Deleting category with id: {id}");
            var result = await _categoryService.DeleteAsync(id);
            if (!result.Success && result.StatusCode == Application.Constants.StatusCodes.NOT_FOUND)
            {
                _logger.LogWarning($"Category delete failed: category not found (id: {id}).");
                return NotFound(result);
            }
            else if (!result.Success && result.StatusCode == Application.Constants.StatusCodes.BAD_REQUEST)
            {
                _logger.LogWarning($"Category delete failed: bad request (id: {id}).");
                return BadRequest(result);
            }
            _logger.LogInformation($"Category with id {id} deleted successfully.");
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoryAsync([FromRoute] string id, [FromBody] CategoryRequestDto requestDto)
        {
            _logger.LogInformation($"Updating category with id: {id}");
            var result = await _categoryService.UpdateAsync(id, requestDto);
            if (!result.Success && result.StatusCode == Application.Constants.StatusCodes.NOT_FOUND)
            {
                _logger.LogWarning($"Category update failed: category not found (id: {id}).");
                return NotFound(result);
            }
            else if (!result.Success && result.StatusCode == Application.Constants.StatusCodes.BAD_REQUEST)
            {
                _logger.LogWarning($"Category update failed: bad request (id: {id}).");
                return BadRequest(result);
            }
            _logger.LogInformation($"Category with id {id} updated successfully.");
            return NoContent();
        }
    }
}
