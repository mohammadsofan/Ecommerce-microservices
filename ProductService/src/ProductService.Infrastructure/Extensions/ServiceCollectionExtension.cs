using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using ProductService.Application.Interfaces;
using ProductService.Application.Interfaces.IRepository;
using ProductService.Infrastructure.Adapters;
using ProductService.Infrastructure.Repositories;

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
            services.AddSingleton<IMongoClient>(sp => new MongoClient(configuration.GetSection("MongoDbSettings:ConnectionString").Value));
            services.AddScoped(typeof(IDbContext<>), typeof(DbContext<>));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IAppLogger<>), typeof(AppLoggerAdapter<>));
            services.AddSingleton<IAppMapper, AppMapperAdapter>();
            return services;
        }
    }
}