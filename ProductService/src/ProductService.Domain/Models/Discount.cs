using MongoDB.Bson.Serialization.Attributes;
using ProductService.Domain.Enums;

namespace ProductService.Domain.Models
{
    public class Discount
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public string? ProductId { get; set; }
        public string? CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DiscountType DiscountType { get; set; }
        public DateTime? ExpirationDate {  get; set; }
    }
}
