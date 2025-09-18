using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoiceService.Application.Dtos.Invoice;
using MediatR;
using Shared.Events;

namespace InvoiceService.Application.Commands
{
    public record class CreateInvoiceCommand(OrderPaidInvoiceEvent Invoice) : IRequest<InvoiceDetailDto>;
    
}