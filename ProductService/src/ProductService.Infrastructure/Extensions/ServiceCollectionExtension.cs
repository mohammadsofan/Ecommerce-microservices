using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using ProductService.Application.Interfaces;
using ProductService.Application.Interfaces.IRepository;
using ProductService.Application.Interfaces.IServices;
using ProductService.Application.Services;
using ProductService.Infrastructure.Adapters;
using ProductService.Infrastructure.Repositories;
using ProductService.Infrastructure.Settings;
using System.Text;

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
            services.Configure<JwtSettings>(options =>
            {
                options.Key = configuration.GetSection("JWT:Key").Value ?? "";
            });
            services.AddSingleton<IMongoClient>(sp => new MongoClient(configuration.GetSection("MongoDbSettings:ConnectionString").Value));
            services.AddScoped(typeof(IDbContext<>), typeof(DbContext<>));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IAppLogger<>), typeof(AppLoggerAdapter<>));
            services.AddSingleton<IAppMapper, AppMapperAdapter>();
            services.AddScoped<IProductService, Application.Services.ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWT:Key").Value!))
                };
            });
            services.AddAuthorization();
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    var rabbitMqUri = new Uri(configuration.GetSection("RabbitMQ:Uri").Value ?? "amqp://localhost:5672");
                    cfg.Host(rabbitMqUri);
                });
            });

            return services;
        }
    }
}