namespace Shared.Events
{
    public class ProductCreatedEvent
    {
        public string Id { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}