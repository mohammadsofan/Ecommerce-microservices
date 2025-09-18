using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InvoiceService.Application.Commands;
using InvoiceService.Application.Dtos.Invoice;
using InvoiceService.Application.IRepository;
using InvoiceService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InvoiceService.Application.Handlers
{
    public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, InvoiceDetailDto>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateInvoiceCommandHandler> _logger;

        public CreateInvoiceCommandHandler(IInvoiceRepository invoiceRepository, IMapper mapper, ILogger<CreateInvoiceCommandHandler> logger)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<InvoiceDetailDto> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var orderPaidInvoiceEvent = request.Invoice;
            try
            {
                var invoice = _mapper.Map<Invoice>(orderPaidInvoiceEvent);
                await _invoiceRepository.AddAsync(invoice);
                var result = _mapper.Map<InvoiceDetailDto>(invoice);
                _logger.LogInformation("Successfully created invoice with ID: {InvoiceId}", invoice.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating invoice for Order ID: {OrderId}", orderPaidInvoiceEvent.OrderId);
                throw;
            }
        }
    }
}