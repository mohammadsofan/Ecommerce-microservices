using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Events;

namespace InvoiceService.Application.Dtos.Invoice
{
    public class InvoiceDetailDto
    {
        
        public string Id { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string AddressId { get; set; } = string.Empty;
        public CustomerInfoDto CustomerInfo { get; set; } = new CustomerInfoDto();
        public AddressDto ShippingAddress { get; set; } = new AddressDto();
        public List<ItemDto> Items { get; set; } = new List<ItemDto>();
        public TotalsDto Totals { get; set; } = new TotalsDto();
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
    }
}