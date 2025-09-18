using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InvoiceService.Application.Dtos.Invoice;
using InvoiceService.Application.IRepository;
using InvoiceService.Application.Queries;
using InvoiceService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace InvoiceService.Application.Handlers
{
    public class GetUserInvoicesQueryHandler : IRequestHandler<GetUserInvoicesQuery, IEnumerable<InvoiceDetailDto>>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetUserInvoicesQueryHandler> _logger;

        public GetUserInvoicesQueryHandler(
            IInvoiceRepository invoiceRepository,
            IMapper mapper,
            ILogger<GetUserInvoicesQueryHandler> logger)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<InvoiceDetailDto>> Handle(GetUserInvoicesQuery request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            try
            {
                var builder = Builders<Invoice>.Filter;
                var filter = builder.Eq(i => i.CustomerId, userId);
                builder.And(filter);
                var invoices = await _invoiceRepository.FindAsync(filter);
                var result = _mapper.Map<IEnumerable<InvoiceDetailDto>>(invoices);
                _logger.LogInformation("Successfully retrieved invoices for user with ID: {UserId}", userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving invoices for user with ID: {UserId}", userId);
                throw;
            }
        }
    }
}