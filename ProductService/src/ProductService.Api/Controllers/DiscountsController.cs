using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Dtos.Discount;
using ProductService.Application.Interfaces.IServices;
using StatusCodes = ProductService.Application.Constants.StatusCodes;

namespace ProductService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountsController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiscountResponseDto>>> GetDiscountsAsync()
        {
            var result = await _discountService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetDiscountByIdAsync")]
        public async Task<ActionResult<DiscountResponseDto>> GetDiscountByIdAsync(string id)
        {
            var result = await _discountService.GetByIdAsync(id);
            if (!result.Success && result.StatusCode == StatusCodes.NOT_FOUND)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DiscountResponseDto>> CreateDiscountAsync([FromBody] DiscountRequestDto discountDto)
        {
            var result = await _discountService.AddAsync(discountDto);
            if (!result.Success)
            {
                if (result.StatusCode == StatusCodes.BAD_REQUEST)
                {
                    return BadRequest(result);
                }
                else if (result.StatusCode == StatusCodes.NOT_FOUND)
                {
                    return NotFound(result);
                }
                else if (result.StatusCode == StatusCodes.CONFLICT)
                {
                    return Conflict(result);
                }
            }
            return CreatedAtRoute(nameof(GetDiscountByIdAsync), new { id = result.Data?.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DiscountResponseDto>> UpdateDiscountAsync(string id, [FromBody] DiscountRequestDto discountDto)
        {
            var result = await _discountService.UpdateAsync(id, discountDto);
            if (!result.Success)
            {
                if (result.StatusCode == StatusCodes.BAD_REQUEST)
                {
                    return BadRequest(result);
                }
                else if (result.StatusCode == StatusCodes.NOT_FOUND)
                {
                    return NotFound(result);
                }
                else if (result.StatusCode == StatusCodes.CONFLICT)
                {
                    return Conflict(result);
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteDiscountAsync(string id)
        {
            var result = await _discountService.DeleteAsync(id);
            if (!result.Success && result.StatusCode == StatusCodes.NOT_FOUND)
            {
                return NotFound(result);
            }
            return NoContent();
        }
    }
}
