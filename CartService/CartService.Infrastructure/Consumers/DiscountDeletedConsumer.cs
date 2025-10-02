using CartService.Application.Interfaces;
using CartService.Application.Interfaces.IRepository;
using MassTransit;
using Shared.Events;

namespace CartService.Infrastructure.Consumers
{
    public class DiscountDeletedConsumer : IConsumer<DiscountDeletedEvent>
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IAppLogger<DiscountDeletedConsumer> _logger;

        public DiscountDeletedConsumer(IDiscountRepository discountRepository, IAppLogger<DiscountDeletedConsumer> logger)
        {
            _discountRepository = discountRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<DiscountDeletedEvent> context)
        {
            try
            {
                var @event = context.Message;
                _logger.LogInformation($"Consuming DiscountDeletedEvent for discount: {@event.DiscountId}");

                var result = await _discountRepository.DeleteByOriginalIdAsync(@event.DiscountId);
                if (result)
                {
                    _logger.LogInformation($"Discount removed from local store: {@event.DiscountId}");
                }
                else
                {
                    _logger.LogWarning($"Discount not found in local store: {@event.DiscountId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error consuming DiscountDeletedEvent", ex);
                throw;
            }
        }
    }
}
