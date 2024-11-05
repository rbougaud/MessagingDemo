using Infrastructure.Persistence.Context;
using MediatR;
using Serilog;

namespace Application.Events.Orders.Order_DeliveryOptionsUpdated;

public class OrderDeliveryOptionsUpdatedEventHandler(ILogger logger, WriterContext context) : INotificationHandler<OrderDeliveryOptionsUpdatedEvent>
{
    private readonly ILogger _logger = logger;
    private readonly WriterContext _context = context;

    public async Task Handle(OrderDeliveryOptionsUpdatedEvent @event, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(OrderDeliveryOptionsUpdatedEventHandler));
        try
        {
            var existingProjection = await _context.OrderProjections.FindAsync(@event.OrderDeliveryOptionsUpdated.Id);
            if (existingProjection is not null)
            {
                existingProjection.State = @event.OrderDeliveryOptionsUpdated.State;
                existingProjection.DeliveryMode = @event.OrderDeliveryOptionsUpdated.DeliveryMode;
                existingProjection.DeliveryDate = @event.OrderDeliveryOptionsUpdated.DeliveryDate;

                _context.OrderProjections.Update(existingProjection);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
        }
    }
}
