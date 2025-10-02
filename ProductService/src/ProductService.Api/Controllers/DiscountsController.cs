using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Dtos.Discount;
using ProductService.Application.Interfaces.IServices;
using Shared.Events;
using StatusCodes = ProductService.Application.Constants.StatusCodes;

namespace ProductService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountService _discountService;
        private readonly IPublishEndpoint _publishEndpoint;

        public DiscountsController(IDiscountService discountService, IPublishEndpoint publishEndpoint)
        {
            _discountService = discountService;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<ActionResult> GetDiscountsAsync()
        {
            var result = await _discountService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetDiscountByIdAsync")]
        public async Task<ActionResult> GetDiscountByIdAsync(string id)
        {
            var result = await _discountService.GetByIdAsync(id);
            if (!result.Success && result.StatusCode == StatusCodes.NOT_FOUND)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        [HttpGet("Product-Or-Category-Id/{id}")]
        public async Task<ActionResult> GetDiscountByProductOrCategoryAsync(string id)
        {
            var result = await _discountService.GetByProductOrCategoryAsync(id);
            if (!result.Success && result.StatusCode == StatusCodes.NOT_FOUND)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateDiscountAsync([FromBody] DiscountRequestDto discountDto)
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
            if (result.Data is not null)
            {
                await _publishEndpoint.Publish(new DiscountCreatedEvent()
                {
                    DiscountId = result.Data!.Id,
                    ProductId = result.Data!.ProductId,
                    CategoryId = result.Data!.CategoryId,
                    Amount = result.Data!.Amount,
                    DiscountType = (Shared.Enums.DiscountType)result.Data!.DiscountType,
                    ExpirationDate = result.Data!.ExpirationDate,
                });
            }
            return CreatedAtRoute(nameof(GetDiscountByIdAsync), new { id = result.Data?.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateDiscountAsync(string id, [FromBody] DiscountRequestDto discountDto)
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
            await _publishEndpoint.Publish(new DiscountUpdatedEvent()
            {
                DiscountId = id,
                ProductId = discountDto.ProductId,
                CategoryId = discountDto.CategoryId,
                Amount = discountDto.Amount,
                DiscountType = (Shared.Enums.DiscountType)discountDto.DiscountType,
                ExpirationDate = discountDto.ExpirationDate,
            });
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
            await _publishEndpoint.Publish(new DiscountDeletedEvent()
            {
                DiscountId = id,
            });
            return NoContent();
        }
    }
}
