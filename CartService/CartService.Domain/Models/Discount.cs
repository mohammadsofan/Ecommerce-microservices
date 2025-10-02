using CartService.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace CartService.Domain.Models
{
    public class Discount
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        [BsonRequired]
        [BsonElement("OriginalId")]
        public string OriginalId { get; set; } = string.Empty;
        public string? ProductId { get; set; }
        public string? CategoryId { get; set; }
        [BsonRequired]
        public decimal Amount { get; set; }
        [BsonRequired]
        public DiscountType DiscountType { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
