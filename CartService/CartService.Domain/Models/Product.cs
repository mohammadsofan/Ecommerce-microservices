using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CartService.Domain.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonRequired]
        [BsonElement("OriginalId")]
        public string OriginalId { get; set; } = string.Empty;

        [BsonRequired]
        public string Name { get; set; } = string.Empty;

        [BsonRequired]
        public decimal Price { get; set; }

        [BsonRequired]
        public bool IsAvailable { get; set; } = true;
    }
}
