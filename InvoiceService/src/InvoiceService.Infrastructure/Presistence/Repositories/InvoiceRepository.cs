using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoiceService.Application.IRepository;
using InvoiceService.Domain.Entities;

namespace InvoiceService.Infrastructure.Presistence.Repositories
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(MongoDbContext context) : base(context)
        {
        }
    }
}