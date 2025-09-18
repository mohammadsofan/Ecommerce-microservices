using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InvoiceService.Application.Dtos.Invoice;
using InvoiceService.Application.Exceptions;
using InvoiceService.Application.IRepository;
using InvoiceService.Application.IServices;
using InvoiceService.Application.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InvoiceService.Application.Handlers
{
    public class DownloadInvoiceDocumentQueryHandler : IRequestHandler<DownloadInvoiceDocumentQuery, DownloadInvoiceDocumentResDto>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceDocumentGenerator _invoiceDocumentGenerator;
        private readonly ILogger<DownloadInvoiceDocumentQueryHandler> _logger;
        private readonly IMapper _mapper;
        public DownloadInvoiceDocumentQueryHandler(
            IInvoiceRepository invoiceRepository,
            IInvoiceDocumentGenerator invoiceDocumentGenerator,
            ILogger<DownloadInvoiceDocumentQueryHandler> logger,
            IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _invoiceDocumentGenerator = invoiceDocumentGenerator;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<DownloadInvoiceDocumentResDto> Handle(DownloadInvoiceDocumentQuery request, CancellationToken cancellationToken)
        {
            var invoiceId = request.InvoiceId;
            try
            {
                var invoice = await _invoiceRepository.GetByIdAsync(invoiceId) ??
                    throw new NotFoundException("Invoice", invoiceId);
                var result = _invoiceDocumentGenerator.GenerateInvoiceDocument(invoice);
                _logger.LogInformation("Successfully downloaded invoice document for invoice ID: {InvoiceId}", invoiceId);
                var response = new DownloadInvoiceDocumentResDto
                {
                    DocumentBytes = result,
                    FileExtension = _invoiceDocumentGenerator.FileExtension,
                    ContentType = _invoiceDocumentGenerator.ContentType
                };
                return response;
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Failed to download invoice with ID {InvoiceId}, {Reason}.", invoiceId, ex.Message);
                throw;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while downloading invoice document with ID: {InvoiceId}", invoiceId);
                throw;
            }
        }
    }
}