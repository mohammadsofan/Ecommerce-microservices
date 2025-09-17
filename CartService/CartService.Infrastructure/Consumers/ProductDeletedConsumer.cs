using CartService.Application.Interfaces;
using CartService.Application.Interfaces.IRepository;
using MassTransit;
using Shared.Events;

namespace CartService.Infrastructure.Consumers
{
    public class ProductDeletedConsumer : IConsumer<ProductDeletedEvent>
    {
        private readonly IProductRepository _productRepository;
        private readonly IAppLogger<ProductDeletedConsumer> _logger;

        public ProductDeletedConsumer(IProductRepository productRepository, IAppLogger<ProductDeletedConsumer> logger)
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
                _logger.LogError("Error consuming ProductDeletedEvent", ex);
                throw;
            }
        }
    }
}
