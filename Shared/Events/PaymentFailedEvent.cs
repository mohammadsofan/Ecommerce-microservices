using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Interfaces;
using Shared.Enums;

namespace Shared.Events
{
    public class PaymentFailedEvent : IInvoiceStatusEvent
    {
        public string InvoiceId { get; set; } = string.Empty;
        public string Status { get; set; } = InvoiceStatus.FAILED.ToString();
    }
}
    