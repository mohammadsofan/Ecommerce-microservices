using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using IdentityService.Application.IServices;
using IdentityService.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
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
            return services;
        }
    }
}