using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoiceService.Application.Dtos.Invoice;
using MediatR;

namespace InvoiceService.Application.Queries
{
    public record GetInvoiceByIdQuery(string Id) : IRequest<InvoiceDetailDto>;
    
}