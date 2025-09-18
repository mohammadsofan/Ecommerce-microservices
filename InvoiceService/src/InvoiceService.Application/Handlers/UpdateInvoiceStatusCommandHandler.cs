using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoiceService.Application.Commands;
using InvoiceService.Application.Exceptions;
using InvoiceService.Application.IRepository;
using InvoiceService.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InvoiceService.Application.Handlers
{
    public class UpdateInvoiceStatusCommandHandler : IRequestHandler<UpdateInvoiceStatusCommand>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ILogger<UpdateInvoiceStatusCommandHandler> _logger;
        public UpdateInvoiceStatusCommandHandler(IInvoiceRepository invoiceRepository, ILogger<UpdateInvoiceStatusCommandHandler> logger)
        {
            _invoiceRepository = invoiceRepository;
            _logger = logger;
        }
        public async Task Handle(UpdateInvoiceStatusCommand request, CancellationToken cancellationToken)
        {
            var invoiceStatusEvent = request.InvoiceStatusEvent;
            try
            {

                var invoice = await _invoiceRepository.GetByIdAsync(invoiceStatusEvent.InvoiceId) ??
                    throw new NotFoundException("Invoice", invoiceStatusEvent.InvoiceId);

                if (!Enum.TryParse(invoiceStatusEvent.Status, true, out InvoiceStatus status))
                {
                    _logger.LogWarning("Invalid status value: {Status} for invoice ID: {InvoiceId}", invoiceStatusEvent.Status, invoiceStatusEvent.InvoiceId);
                    throw new Exceptions.InvalidOperationException($"Invalid status value: {invoiceStatusEvent.Status}");
                }
                if (invoice.Status == status.ToString())
                {
                    _logger.LogInformation("No status change for invoice ID: {InvoiceId}. Current status is already {Status}", invoice.Id, invoice.Status);
                    return;
                }

                invoice.Status = status.ToString();
                await _invoiceRepository.UpdateAsync(invoice);
                _logger.LogInformation("Successfully updated status for invoice ID: {InvoiceId} to {Status}", invoice.Id, invoice.Status);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Failed to update status for invoice ID: {InvoiceId}, {Reason}.", invoiceStatusEvent.InvoiceId, ex.Message);
                throw;
            }
            catch (Exceptions.InvalidOperationException ex)
            {
                _logger.LogWarning("Failed to update status for invoice ID: {InvoiceId}, {Reason}.", invoiceStatusEvent.InvoiceId, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating status for invoice ID: {InvoiceId}", invoiceStatusEvent.InvoiceId);
                throw;
            }

        }
    }
}