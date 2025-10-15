using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using OrderService.Application.Interfaces;
using OrderService.Infrastructure.Adapters;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Settings;
using System.Text;

namespace OrderService.Infrastructure.Extensions
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
            return services;
        }
    }
}
