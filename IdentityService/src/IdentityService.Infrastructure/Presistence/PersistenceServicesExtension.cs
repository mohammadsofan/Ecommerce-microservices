using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Application.IRepository;
using IdentityService.Infrastructure.Presistence.Data;
using IdentityService.Infrastructure.Presistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Infrastructure.Presistence
{
    public static class PersistenceServicesExtension
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<MongoDbContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
        public static async Task SeedDatabase(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MongoDbContext>();

            await DataSeeder.SeedAsync(db);
        }
    }
}