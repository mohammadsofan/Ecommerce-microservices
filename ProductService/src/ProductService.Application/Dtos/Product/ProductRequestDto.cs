using System.ComponentModel.DataAnnotations;

namespace ProductService.Application.Dtos.Product
{
    public class ProductRequestDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(3, ErrorMessage = "Name length must be at least 3 characters")]
        [MaxLength(50, ErrorMessage = "Name length must not be more than 50 characters")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MinLength(3, ErrorMessage = "Description length must be at least 3 characters")]
        [MaxLength(200, ErrorMessage = "Description length must not be more than 200 characters")]
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "CategoryId is required")]
        public string CategoryId { get; set; } = string.Empty;
        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Stock is required")]
        public int Stock { get; set; }
    }
}
