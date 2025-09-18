using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoiceService.Domain.Enums;

namespace InvoiceService.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public string OrderId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string AddressId { get; set; } = string.Empty;
        public CustomerInfo CustomerInfo { get; set; } = new CustomerInfo();
        public Address ShippingAddress { get; set; } = new Address();
        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
        public Totals Totals { get; set; } = new Totals();
        public string Status { get; set; } = InvoiceStatus.ISSUED.ToString();
        public DateTime? DueDate { get; set; }

    }
    public class CustomerInfo
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
    public class Address
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
    public class InvoiceItem
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; } 
    }
    public class Totals
    {
        // public decimal Subtotal { get; set; }
        // public decimal Tax { get; set; }
        // public decimal Shipping { get; set; }
        public decimal Total { get; set; }
    }
    
}