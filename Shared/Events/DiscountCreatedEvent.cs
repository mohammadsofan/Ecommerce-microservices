using Shared.Enums;

namespace Shared.Events
{
    public class DiscountCreatedEvent
    {
        public string DiscountId { get; set; } = string.Empty;
        public string? ProductId { get; set; }
        public string? CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DiscountType DiscountType { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}