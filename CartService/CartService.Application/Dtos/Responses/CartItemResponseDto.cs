namespace CartService.Application.Dtos.Responses
{
    public class CartItemResponseDto
    {
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
