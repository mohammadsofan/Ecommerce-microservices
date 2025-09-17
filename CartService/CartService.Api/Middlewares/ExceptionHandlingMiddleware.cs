
using CartService.Application.Constants;
using CartService.Application.Interfaces;
using CartService.Application.Wrappers;
using System;
using System.Text.Json;

namespace CartService.Api.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly IAppLogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(IAppLogger<ExceptionHandlingMiddleware> logger) {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex) {
                _logger.LogError("Excpetion occured", ex);
                context.Response.StatusCode = Application.Constants.StatusCodes.INTERNAL_SERVER_ERROR;
                context.Response.ContentType = "application/json";
                var response = ServiceResult.Fail(
                    Application.Constants.StatusCodes.INTERNAL_SERVER_ERROR,
                    "An unexpected error occurred. Please try again later.");
                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
