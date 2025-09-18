using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoiceService.Application.Dtos.Invoice;
using MediatR;

namespace InvoiceService.Application.Queries
{
    public record class FilterInvoicesQuery(FilterInvoicesDto Filter) : IRequest<IEnumerable<InvoiceDetailDto>>;

}