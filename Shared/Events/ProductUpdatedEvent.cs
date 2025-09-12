namespace Shared.Events
{
    public class ProductUpdatedEvent
    {
        public string Id { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}