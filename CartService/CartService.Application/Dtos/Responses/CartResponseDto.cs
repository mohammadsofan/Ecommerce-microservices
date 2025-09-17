namespace CartService.Application.Dtos.Responses
{
    public class CartResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public List<CartItemResponseDto> Items { get; set; } = new List<CartItemResponseDto>();
        public decimal TotalPrice { get; set; }
    }
}
