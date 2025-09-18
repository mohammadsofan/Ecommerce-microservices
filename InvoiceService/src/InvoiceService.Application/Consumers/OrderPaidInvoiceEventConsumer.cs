using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InvoiceService.Application.Commands;
using InvoiceService.Application.IServices;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Events;

namespace InvoiceService.Application.Consumers
{
    public class OrderPaidInvoiceEventConsumer : IConsumer<OrderPaidInvoiceEvent>
    {
        private readonly ILogger<OrderPaidInvoiceEventConsumer> _logger;
        private readonly IMediator _mediator;

        public OrderPaidInvoiceEventConsumer(
            ILogger<OrderPaidInvoiceEventConsumer> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        public async Task Consume(ConsumeContext<OrderPaidInvoiceEvent> context)
        {
            try
            {
                _logger.LogInformation("Order placed event received: {OrderId}", context.Message.OrderId);
                var result = await _mediator.Send(new CreateInvoiceCommand(context.Message));
                _logger.LogInformation("Invoice created for order: {OrderId} at {CreatedAt}", context.Message.OrderId, result.CreatedAt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing order placed event: {OrderId}", context.Message.OrderId);
                throw;
            }
        }
    }
}
          