using CartService.Application.Dtos.Requests;
using CartService.Application.Interfaces.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CartService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }
        [HttpGet]
        public async Task<IActionResult> GetCartByUserId()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var cart = await cartService.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }
        [HttpPost]
        public async Task<IActionResult> AddItemToCart([FromBody] AddCartItemRequestDto item)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var result = await cartService.AddItemToCartAsync(userId, item);
            if (!result.Success && result.StatusCode == Application.Constants.StatusCodes.BAD_REQUEST)
            {
                return BadRequest(result);
            }
            else if (!result.Success && result.StatusCode == Application.Constants.StatusCodes.NOT_FOUND)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveItemFromCart(string productId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var result = await cartService.RemoveItemFromCartAsync(userId, productId);
            if (!result.Success && result.StatusCode == Application.Constants.StatusCodes.BAD_REQUEST)
            {
                return BadRequest(result);
            }
            else if (!result.Success && result.StatusCode == Application.Constants.StatusCodes.NOT_FOUND)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        [HttpPut("{productId}/quantity/{quantity}")]
        public async Task<IActionResult> UpdateItemQuantity(string productId, int quantity)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var result = await cartService.UpdateItemQuantityAsync(userId, productId, quantity);
            if (!result.Success && result.StatusCode == Application.Constants.StatusCodes.BAD_REQUEST)
            {
                return BadRequest(result);
            }
            else if (!result.Success && result.StatusCode == Application.Constants.StatusCodes.NOT_FOUND)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

    }
}
