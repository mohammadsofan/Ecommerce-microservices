using EmailService.Api.Extensions;
using EmailService.Api.Interfaces;
using EmailService.Api.Models;
using EmailService.Api.Services;
using EmailService.Api.Templates;
using Microsoft.Extensions.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// configure strongly typed settings objects
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));

// Add services to the container.
builder.Services.AddOpenApi();

// Add RabbitMQ service
builder.Services.AddRabbitMqService(builder.Configuration);
// Add other services
builder.Services.AddScoped<ITemplateBuilder, BaseTemplateBuilder>();
builder.Services.AddScoped<IEmailSender, EmailSender>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();

app.Run();

