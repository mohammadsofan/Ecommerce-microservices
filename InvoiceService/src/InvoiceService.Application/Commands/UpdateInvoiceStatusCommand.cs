using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Shared.Interfaces;

namespace InvoiceService.Application.Commands
{
    public record class UpdateInvoiceStatusCommand(IInvoiceStatusEvent InvoiceStatusEvent) : IRequest;
}