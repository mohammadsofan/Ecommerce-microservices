using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductService.Domain.Models{
    public class Product{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        [BsonRequired]
        public string Name { get; set; } = string.Empty;
        [BsonRequired]
        public string Description { get; set; } = string.Empty;
        [BsonRequired]
        public decimal Price { get; set; }
        [BsonRequired]
        public int Stock { get; set; }
    }
}