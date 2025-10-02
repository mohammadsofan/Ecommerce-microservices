using CartService.Application.Interfaces;
using CartService.Application.Interfaces.IRepository;
using CartService.Application.Interfaces.IService;
using CartService.Infrastructure.Adapters;
using CartService.Infrastructure.Consumers;
using CartService.Infrastructure.Data;
using CartService.Infrastructure.Repositories;
using CartService.Infrastructure.Settings;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;

namespace CartService.Infrastructure.Extensions
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
            services.AddScoped(typeof(IAppLogger<>), typeof(AppLoggerAdapter<>));
            services.AddSingleton<IMongoClient>(sp => new MongoClient(configuration.GetSection("MongoDbSettings:ConnectionString").Value));
            services.AddScoped(typeof(IDbContext<>), typeof(DbContext<>));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartService, Application.Services.CartService>();
            services.AddSingleton<IAppMapper, AppMapperAdapter>();
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
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddMassTransit(config =>
            {
                config.AddConsumer<ProductCreatedConsumer>();
                config.AddConsumer<ProductDeletedConsumer>();
                config.AddConsumer<DiscountCreatedConsumer>();
                config.AddConsumer<DiscountDeletedConsumer>();
                config.AddConsumer<DiscountUpdatedConsumer>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    var rabbitMqUri = new Uri(configuration.GetSection("RabbitMQ:Uri").Value ?? "amqp://localhost:5672");
                    cfg.Host(rabbitMqUri);

                    cfg.ReceiveEndpoint("cart-service-product-created", e =>
                    {
                        e.ConfigureConsumer<ProductCreatedConsumer>(ctx);
                    });

                    cfg.ReceiveEndpoint("cart-service-product-deleted", e =>
                    {
                        e.ConfigureConsumer<ProductDeletedConsumer>(ctx);
                    });
                    cfg.ReceiveEndpoint("cart-service-discount-created", e =>
                    {
                        e.ConfigureConsumer<DiscountCreatedConsumer>(ctx);
                    });
                    cfg.ReceiveEndpoint("cart-service-discount-updated", e =>
                    {
                        e.ConfigureConsumer<DiscountUpdatedConsumer>(ctx);
                    });
                    cfg.ReceiveEndpoint("cart-service-discount-deleted", e =>
                    {
                        e.ConfigureConsumer<DiscountDeletedConsumer>(ctx);
                    });
                });
            });
            return services;
        }
    }
}
