using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Events
{
    public record class OrderPaidInvoiceEvent
    {
        public string OrderId { get; init; } = string.Empty;
        public string CustomerId { get; init; } = string.Empty;
        public string AddressId { get; init; } = string.Empty;
        public CustomerInfoDto CustomerInfo { get; init; } = new CustomerInfoDto();
        public AddressDto ShippingAddress { get; init; } = new AddressDto();
        public List<ItemDto> Items { get; init; } = new List<ItemDto>();
        public TotalsDto Totals { get; init; } = new TotalsDto();
        public string Status { get; init; } = string.Empty;
        public DateTime? DueDate { get; init; }
    }

    public class CustomerInfoDto
    {
        public string FullName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
    }
    public class AddressDto
    {
        public string Street { get; init; } = string.Empty;
        public string City { get; init; } = string.Empty;
        public string State { get; init; } = string.Empty;
        public string PostalCode { get; init; } = string.Empty;
        public string Country { get; init; } = string.Empty;
    }
    public class ItemDto
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public int Quantity { get; init; }
        public decimal UnitPrice { get; init; }
        public decimal TotalPrice { get; init; }
    }
    public class TotalsDto
    {
        // public decimal Subtotal { get; init; }
        // public decimal Tax { get; init; }
        // public decimal Shipping { get; init; }
        public decimal Total { get; init; }
    }


}