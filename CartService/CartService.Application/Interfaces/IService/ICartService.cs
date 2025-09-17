using CartService.Application.Dtos.Requests;
using CartService.Application.Dtos.Responses;
using CartService.Application.Interfaces.IServices;
using CartService.Application.Wrappers;
using CartService.Domain.Models;

namespace CartService.Application.Interfaces.IService
{
    public interface ICartService:IGenericService<CartRequestDto,CartResponseDto,Cart>
    {
        Task<ServiceResult<CartResponseDto>> GetCartByUserIdAsync(string userId);
        Task<ServiceResult<CartResponseDto>> AddItemToCartAsync(string userId, AddCartItemRequestDto item);
        Task<ServiceResult> RemoveItemFromCartAsync(string userId, string productId);
        Task<ServiceResult> UpdateItemQuantityAsync(string userId, string productId, int quantity);
        Task<ServiceResult> ClearCartAsync(string userId);
    }
}
