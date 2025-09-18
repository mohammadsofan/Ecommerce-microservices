using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceService.Application.Dtos.Invoice
{
    public class FilterInvoicesDto
    {
        public string? CustomerName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public string? Status { get; set; }
        public string? ShippingCity { get; set; }
        public string? ShippingCountry { get; set; }
        
    }
}