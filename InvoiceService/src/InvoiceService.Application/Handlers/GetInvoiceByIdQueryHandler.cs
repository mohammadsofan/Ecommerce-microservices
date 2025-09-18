using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InvoiceService.Application.Dtos.Invoice;
using InvoiceService.Application.Exceptions;
using InvoiceService.Application.IRepository;
using InvoiceService.Application.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InvoiceService.Application.Handlers
{
    public class GetInvoiceByIdQueryHandler : IRequestHandler<GetInvoiceByIdQuery, InvoiceDetailDto>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetInvoiceByIdQueryHandler> _logger;

        public GetInvoiceByIdQueryHandler(
            IInvoiceRepository invoiceRepository,
            IMapper mapper,
            ILogger<GetInvoiceByIdQueryHandler> logger)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<InvoiceDetailDto> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
        {
            var invoiceId = request.Id;
            try
            {
                var invoice = await _invoiceRepository.GetByIdAsync(invoiceId) ??
                    throw new NotFoundException("Invoice", invoiceId);

                var result = _mapper.Map<InvoiceDetailDto>(invoice);
                _logger.LogInformation("Successfully retrieved invoice with ID: {InvoiceId}", invoiceId);
                return result;
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Failed to retrieve invoice with ID: {InvoiceId}, {Reason}.", invoiceId, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving invoice with ID: {InvoiceId}", invoiceId);
                throw;
            }
        }
    }
}