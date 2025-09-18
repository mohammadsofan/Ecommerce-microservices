using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Shared.Interfaces
{
    public interface IInvoiceStatusEvent
    {
        public string InvoiceId { get; set; } 
        public string Status { get; set; } 
    }
    
}