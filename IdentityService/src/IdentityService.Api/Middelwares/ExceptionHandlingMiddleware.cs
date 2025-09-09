using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IdentityService.Application.Exceptions;

namespace IdentityService.Api.Middelwares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (FluentValidation.ValidationException ex)
            {
                _logger.LogError(ex, "A validation exception has occurred.");
                await HandleExceptionAsync(context, new ValidationException(ex.Errors));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            string title = string.Empty;
            HttpStatusCode statusCode;
            string detail = exception.Message;

            switch (exception)
            {
                case InvalidCredentialsException:
                    title = "Unauthorized";
                    statusCode = HttpStatusCode.Unauthorized;
                    break;
                case NotFoundException:
                case KeyNotFoundException:
                    title = "Not Found";
                    statusCode = HttpStatusCode.NotFound;
                    break;
                case Application.Exceptions.InvalidOperationException:
                case System.InvalidOperationException:
                case ValidationException:
                case ArgumentException:
                case BadHttpRequestException:
                case InactiveUserException:
                    title = "Bad Request";
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                default:
                    title = "Internal Server Error";
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }
            var problemDetails = new
            {
                type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                title,
                status = (int)statusCode,
                detail
            };
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}