using Microsoft.AspNetCore.Mvc;
using ProductService.Api.Middleware;
using ProductService.Application.Wrappers;
using ProductService.Infrastructure.Data;
using Serilog;
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(config)
    .CreateBootstrapLogger();
var logger = Log.ForContext<Program>();
logger.Information("Starting up Api");
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((context, services, config) =>
    {
        config.ReadFrom.Configuration(context.Configuration);
        config.ReadFrom.Services(services)
        .Enrich.FromLogContext();
    });

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value!.Errors.Count > 0)
                .SelectMany(e => e.Value!.Errors.Select(er => new Error
                {
                    Field = e.Key,
                    Message = er.ErrorMessage
                }))
                .ToList();

            var result = ServiceResult.Fail(
                ProductService.Application.Constants.StatusCodes.BAD_REQUEST,
                "Validation failed.",
                errors
            );
            return new BadRequestObjectResult(result);
        };
    });

    var app = builder.Build();

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();
    app.MapControllers();
    app.UseAuthentication();
    app.UseAuthorization();
    app.Run();

}
catch (Exception ex)
{
    logger.Fatal(ex, "An unhandled exception occurred during bootstrapping");
}
finally
{
    Log.CloseAndFlush();
}