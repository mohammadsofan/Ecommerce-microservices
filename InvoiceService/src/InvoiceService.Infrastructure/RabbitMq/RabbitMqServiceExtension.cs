using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoiceService.Infrastructure.Configurations;
using InvoiceService.Application.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceService.Infrastructure.Extensions
{
    public static class RabbitMqServiceExtension
    {
        public static void AddRabbitMqService(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMqSettings = configuration.GetSection("RabbitMQ").Get<RabbitMqSettings>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<OrderPaidInvoiceEventConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqSettings!.Host, h =>
                    {
                        h.Username(rabbitMqSettings.Username);
                        h.Password(rabbitMqSettings.Password);
                    });

                    cfg.ReceiveEndpoint("invoice-service.order-paid", e =>
                    {
                        e.ConfigureConsumer<OrderPaidInvoiceEventConsumer>(context);
                    });
                });
            });
        }
        
    }
}