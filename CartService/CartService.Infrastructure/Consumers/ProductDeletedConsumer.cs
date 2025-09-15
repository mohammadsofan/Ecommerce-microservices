using CartService.Application.Interfaces.IRepository;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Events;

namespace CartService.Infrastructure.Consumers
{
    public class ProductDeletedConsumer : IConsumer<ProductDeletedEvent>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductDeletedConsumer> _logger;

        public ProductDeletedConsumer(IProductRepository productRepository, ILogger<ProductDeletedConsumer> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
        {
            try
            {
                var @event = context.Message;
                _logger.LogInformation($"Consuming ProductDeletedEvent for product: {@event.Id}");

                var result = await _productRepository.DeleteByOriginalIdAsync(@event.Id);
                if (result)
                {
                    _logger.LogInformation($"Product removed from local store: {@event.Id}");
                }
                else
                {
                    _logger.LogWarning($"Product not found in local store: {@event.Id}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consuming ProductDeletedEvent");
                throw;
            }
        }
    }
}
