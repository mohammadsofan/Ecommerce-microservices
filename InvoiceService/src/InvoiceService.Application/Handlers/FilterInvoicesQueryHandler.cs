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
    public class FilterInvoicesQueryHandler : IRequestHandler<FilterInvoicesQuery, IEnumerable<InvoiceDetailDto>>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ILogger<FilterInvoicesQueryHandler> _logger;
        private readonly IMapper _mapper;
        public FilterInvoicesQueryHandler(
            IInvoiceRepository invoiceRepository,
            IMapper mapper,
            ILogger<FilterInvoicesQueryHandler> logger)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IEnumerable<InvoiceDetailDto>> Handle(FilterInvoicesQuery request, CancellationToken cancellationToken)
        {
            var filter = request.Filter;
            try
            {
                var mongoFilter = BuildFilter(filter);
                var invoices = await _invoiceRepository.FindAsync(mongoFilter);
                var result = _mapper.Map<IEnumerable<InvoiceDetailDto>>(invoices);
                _logger.LogInformation("Fetched {Count} invoices with applied filters {@FilterDto}", result.Count(), filter);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while filtering invoices.");
                throw;
            }
        }
        private FilterDefinition<Invoice> BuildFilter(FilterInvoicesDto filter)
        {
            var builder = Builders<Invoice>.Filter;
            var filters = new List<FilterDefinition<Invoice>>();

            if (!string.IsNullOrEmpty(filter.CustomerName))
            {
                filters.Add(builder.Regex(i => i.CustomerInfo.FullName, new MongoDB.Bson.BsonRegularExpression(filter.CustomerName, "i")));
            }
            if (filter.FromDate.HasValue)
            {
                filters.Add(builder.Gte(i => i.CreatedAt, filter.FromDate.Value));
            }
            if (filter.ToDate.HasValue)
            {
                filters.Add(builder.Lte(i => i.CreatedAt, filter.ToDate.Value));
            }
            if (filter.MinAmount.HasValue)
            {
                filters.Add(builder.Gte(i => i.Totals.Total, filter.MinAmount.Value));
            }
            if (filter.MaxAmount.HasValue)
            {
                filters.Add(builder.Lte(i => i.Totals.Total, filter.MaxAmount.Value));
            }
            if (!string.IsNullOrEmpty(filter.Status))
            {
                filters.Add(builder.Eq(i => i.Status, filter.Status));
            }
            if (!string.IsNullOrEmpty(filter.ShippingCity))
            {
                filters.Add(builder.Regex(i => i.ShippingAddress.City, new MongoDB.Bson.BsonRegularExpression(filter.ShippingCity, "i")));
            }
            if (!string.IsNullOrEmpty(filter.ShippingCountry))
            {
                filters.Add(builder.Regex(i => i.ShippingAddress.Country, new MongoDB.Bson.BsonRegularExpression(filter.ShippingCountry, "i")));
            }

            return filters.Count > 0 ? builder.And(filters) : builder.Empty;
        }
    }
}