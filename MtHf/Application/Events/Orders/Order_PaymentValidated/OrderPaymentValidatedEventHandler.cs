using Application.Events.Orders.Order_DeliveryOptionsUpdated;
using Infrastructure.Persistence.Context;
using MediatR;
using Serilog;

namespace Application.Events.Orders.Order_PaymentValidated;

public class OrderPaymentValidatedEventHandler(ILogger logger, WriterContext context) : INotificationHandler<OrderPaymentValidatedEvent>
{
    private readonly ILogger _logger = logger;
    private readonly WriterContext _context = context;

    public async Task Handle(OrderPaymentValidatedEvent @event, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(OrderDeliveryOptionsUpdatedEventHandler));
        try
        {
            var existingProjection = await _context.OrderProjections.FindAsync(@event.OrderPaymentValidated.Id);
            if (existingProjection is not null)
            {
                existingProjection.PaymentMode = @event.OrderPaymentValidated.PaymentMode;
                existingProjection.State = @event.OrderPaymentValidated.State;

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
