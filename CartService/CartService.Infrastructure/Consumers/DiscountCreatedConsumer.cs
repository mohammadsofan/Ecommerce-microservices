using CartService.Application.Interfaces;
using CartService.Application.Interfaces.IRepository;
using CartService.Domain.Models;
using MassTransit;
using Shared.Events;

namespace CartService.Infrastructure.Consumers
{
    public class DiscountCreatedConsumer : IConsumer<DiscountCreatedEvent>
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IAppLogger<DiscountCreatedConsumer> _logger;

        public DiscountCreatedConsumer(IDiscountRepository discountRepository, IAppLogger<DiscountCreatedConsumer> logger)
        {
            _discountRepository = discountRepository;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<DiscountCreatedEvent> context)
        {
            try
            {
                var @event = context.Message;
                _logger.LogInformation($"Consuming DiscountCreatedEvent for Discount: {@event.DiscountId}");

                var discount = new Discount
                {
                    OriginalId = @event.DiscountId,
                    ProductId = @event.ProductId,
                    CategoryId = @event.CategoryId,
                    Amount = @event.Amount,
                    DiscountType = (Domain.Enums.DiscountType)@event.DiscountType,
                    ExpirationDate = @event.ExpirationDate
                };

                await _discountRepository.AddAsync(discount);
                _logger.LogInformation($"Discount added to local store: {@event.DiscountId}");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error consuming DiscountCreatedEvent", ex);
                throw;
            }
        }
    }
}
