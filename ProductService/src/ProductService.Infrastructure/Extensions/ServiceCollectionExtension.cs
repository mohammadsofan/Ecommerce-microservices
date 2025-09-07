using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProductService.Infrastructure.Data
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbSettings>(options =>
            {
                options.ConnectionString = configuration.GetSection("MongoDbSettings:ConnectionString").Value ?? "";
                options.DatabaseName = configuration.GetSection("MongoDbSettings:DatabaseName").Value ?? "";
            });
            services.AddSingleton<DbContext>();
            return services;
        }
    }
}