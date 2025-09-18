using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoiceService.Application.IRepository;
using InvoiceService.Infrastructure.Presistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceService.Infrastructure.Presistence
{
    public static class PersistenceServicesExtension
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<MongoDbContext>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            
            return services;
        }
      
    }
}