using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CartService.Domain.Models
{
    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        [BsonRequired]
        public string UserId { get; set; } = string.Empty;
        public IList<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
