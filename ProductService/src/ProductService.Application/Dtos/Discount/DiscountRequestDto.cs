using ProductService.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProductService.Application.Dtos.Discount
{
    public class DiscountRequestDto
    {
        public string? ProductId { get; set; }
        public string? CategoryId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DiscountType DiscountType { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
