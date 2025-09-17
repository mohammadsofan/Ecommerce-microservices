using MongoDB.Bson.Serialization.Attributes;

namespace CartService.Application.Dtos.Requests
{
    public class AddCartItemRequestDto
    {
        [BsonRequired]
        public string ProductId { get; set; } = string.Empty;
        [BsonRequired]
        public int Quantity { get; set; }
    }
}
