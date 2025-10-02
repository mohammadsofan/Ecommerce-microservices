using CartService.Application.Interfaces;
using CartService.Application.Interfaces.IRepository;
using CartService.Domain.Models;
using MassTransit;
using Shared.Events;

namespace CartService.Infrastructure.Consumers
{
    public class DiscountUpdatedConsumer : IConsumer<DiscountUpdatedEvent>
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IAppLogger<DiscountUpdatedConsumer> _logger;

        public DiscountUpdatedConsumer(IDiscountRepository discountRepository,IAppLogger<DiscountUpdatedConsumer> logger)
        {
            _discountRepository = discountRepository;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<DiscountUpdatedEvent> context)
        {
            try
            {
                var @event = context.Message;
                _logger.LogInformation($"Consuming DiscountUpdatedEvent for Discount: {@event.DiscountId}");
                var existingDiscount = await _discountRepository.GetByOriginalIdAsync(@event.DiscountId);
                if (existingDiscount != null)
                {
                    var discount = new Discount
                    {
                        OriginalId = @event.DiscountId,
                        ProductId = @event.ProductId,
                        CategoryId = @event.CategoryId,
                        Amount = @event.Amount,
                        DiscountType = (Domain.Enums.DiscountType)@event.DiscountType,
                        ExpirationDate = @event.ExpirationDate
                    };

                    await _discountRepository.UpdateAsync(existingDiscount.Id, discount);
                    _logger.LogInformation($"Discount Updated in local store: {@event.DiscountId}");
                }
                else
                {
                    _logger.LogWarning($"Discount with OriginalId: {@event.DiscountId} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error consuming DiscountCreatedEvent", ex);
                throw;
            }
        }
    }
}
