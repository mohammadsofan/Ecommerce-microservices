using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InvoiceService.Application.Dtos.Invoice;
using InvoiceService.Domain.Entities;
using Shared.Events;

namespace InvoiceService.Application.Mappings
{
    public class InvoiceMappingProfile : Profile

    {
        public InvoiceMappingProfile()
        {
            CreateMap<Invoice, InvoiceDetailDto>();
            CreateMap<CustomerInfo, CustomerInfoDto>();
            CreateMap<Address, AddressDto>();
            CreateMap<InvoiceItem, ItemDto>();
            CreateMap<Totals, TotalsDto>();
            CreateMap<OrderPaidInvoiceEvent, Invoice>();
       
        }
    }
}