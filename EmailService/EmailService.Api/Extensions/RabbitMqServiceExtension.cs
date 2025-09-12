using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailService.Api.Consumers;
using EmailService.Api.Models;
using MassTransit;

namespace EmailService.Api.Extensions
{
    public static class RabbitMqServiceExtension
    {
        public static void AddRabbitMqService(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMqSettings = configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>();
            
            services.AddMassTransit(x =>
            {
                x.AddConsumer<ForgotPasswordConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqSettings!.Host, h =>
                    {
                        h.Username(rabbitMqSettings.Username);
                        h.Password(rabbitMqSettings.Password);
                    });

                    cfg.ReceiveEndpoint("email-service.forgotpassword", e =>
                    {
                        e.ConfigureConsumer<ForgotPasswordConsumer>(context);
                    });
                });
            });
        }
        
    }
}