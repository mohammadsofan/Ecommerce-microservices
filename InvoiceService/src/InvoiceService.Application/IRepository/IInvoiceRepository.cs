using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoiceService.Domain.Entities;

namespace InvoiceService.Application.IRepository
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        
    }
}