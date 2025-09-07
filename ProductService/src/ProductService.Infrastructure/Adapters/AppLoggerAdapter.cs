using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using ProductService.Application.Interfaces;

namespace ProductService.Infrastructure.Adapters
{
    internal class AppLoggerAdapter<T> : IAppLogger<T>
    {
        private readonly ILogger<T> _logger;

        public AppLoggerAdapter(ILogger<T> logger)
        {
            _logger = logger;
        }
        public void LogError(string message, Exception ex) => _logger.LogError(ex, message);

        public void LogInformation(string message) => _logger.LogInformation(message);

        public void LogWarning(string message) => _logger.LogWarning(message);
        public void LogDebug(string message) => _logger.LogDebug(message);
    }
}
