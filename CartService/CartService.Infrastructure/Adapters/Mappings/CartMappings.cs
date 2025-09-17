using CartService.Application.Dtos.Responses;
using CartService.Domain.Models;
using Mapster;

namespace CartService.Infrastructure.Adapters.Mappings
{
    public class CartMappings : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Cart, CartResponseDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest.Items, src => src.Items)
                .Map(dest => dest.TotalPrice, src => src.Items.Sum(item => item.Price * item.Quantity));

            config.NewConfig<CartItem, CartItemResponseDto>()
                .Map(dest => dest.ProductId, src => src.ProductId)
                .Map(dest => dest.Quantity, src => src.Quantity)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.Subtotal, src => src.Price * src.Quantity);
        }
    }
}
