using MongoDB.Bson.Serialization.Attributes;

namespace CartService.Domain.Models
{
    public class CartItem
    {
        [BsonRequired]
        public string ProductId { get; set; } = string.Empty;

        [BsonRequired]
        public int Quantity { get; set; }

        [BsonRequired]
        public decimal Price { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
