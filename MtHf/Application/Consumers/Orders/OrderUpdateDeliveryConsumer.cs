using Application.Events.Orders.Order_DeliveryOptionsUpdated;
using Contracts.Order;
using MassTransit;
using MediatR;
using Serilog;

namespace Application.Consumers.Orders;

public class OrderUpdateDeliveryConsumer(ILogger logger, IMediator mediator) : IConsumer<OrderDeliveryOptionsUpdated>
{
    private readonly ILogger _logger = logger;
    private readonly IMediator _mediator = mediator;

    public async Task Consume(ConsumeContext<OrderDeliveryOptionsUpdated> context)
    {
        _logger.Information(nameof(OrderUpdateDeliveryConsumer));
        await _mediator.Publish(new OrderDeliveryOptionsUpdatedEvent(context.Message));
    }
}
