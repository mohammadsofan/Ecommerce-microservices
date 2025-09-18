using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoiceService.Application.Commands;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;

namespace InvoiceService.Application.Consumers
{
    public class InvoiceStatusUpdateConsumer : IConsumer<IInvoiceStatusEvent>
    {
        private readonly ILogger<InvoiceStatusUpdateConsumer> _logger;
        private readonly IMediator _mediator;
        public InvoiceStatusUpdateConsumer(
            ILogger<InvoiceStatusUpdateConsumer> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        public async Task Consume(ConsumeContext<IInvoiceStatusEvent> context)
        {
            try
            {
                _logger.LogInformation("Invoice status update event received: {InvoiceId}", context.Message.InvoiceId);
                await _mediator.Send(new UpdateInvoiceStatusCommand(context.Message));
                _logger.LogInformation("Invoice status updated for invoice: {InvoiceId} to {Status}", context.Message.InvoiceId, context.Message.Status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing invoice status update event: {InvoiceId}", context.Message.InvoiceId);
                throw;
            }
        }
    }
}