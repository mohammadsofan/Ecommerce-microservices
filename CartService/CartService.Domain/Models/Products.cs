using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CartService.Domain.Models
{
    public class Products
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        [BsonRequired]
        public decimal Price { get; set; }

    }
}
