using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoiceService.Application.IServices;
using InvoiceService.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IInvoiceDocumentGenerator, PdfInvoiceDocumentGenerator>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            return services;
        }
        
    }
}