using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using IdentityService.Application.IServices;
using IdentityService.Application.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
        {

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddValidatorsFromAssembly(AssemblyReference.Assembly);
            services.AddAutoMapper(cfg=> {},AssemblyReference.Assembly);
            services.AddFluentValidationAutoValidation(config =>
                {
                    config.DisableDataAnnotationsValidation = true; 
                });
            services.AddFluentValidationClientsideAdapters();
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration.GetValue<string>("RabbitMq:Host"), h =>
                    {
                        h.Username(configuration.GetValue<string>("RabbitMq:Username")!);
                        h.Password(configuration.GetValue<string>("RabbitMq:Password")!);
                    });
                });
            });

            return services;
        }
    }
}