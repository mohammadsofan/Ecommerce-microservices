using CartService.Application.Interfaces;
using CartService.Application.Interfaces.IRepository;
using CartService.Domain.Models;
using MassTransit;
using Shared.Events;

namespace CartService.Infrastructure.Consumers
{
    public class ProductCreatedConsumer : IConsumer<ProductCreatedEvent>
    {
        private readonly IProductRepository _productRepository;
        private readonly IAppLogger<ProductCreatedConsumer> _logger;

        public ProductCreatedConsumer(IProductRepository productRepository, IAppLogger<ProductCreatedConsumer> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
        {
            try
            {
                var @event = context.Message;
                _logger.LogInformation($"Consuming ProductCreatedEvent for product: {@event.Id}");

                var product = new Product
                {
                    OriginalId = @event.Id,
                    CategoryId = @event.CategoryId,
                    Price = @event.Price,
                    IsAvailable = true
                };

                await _productRepository.AddAsync(product);
                _logger.LogInformation($"Product added to local store: {@event.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error consuming ProductCreatedEvent", ex);
                throw;
            }
        }
    }
}
