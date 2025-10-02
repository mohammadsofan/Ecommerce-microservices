using CartService.Domain.Enums;

namespace CartService.Application.Dtos.Discount
{
    public class DiscountResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string? ProductId { get; set; }
        public string? CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DiscountType DiscountType { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
