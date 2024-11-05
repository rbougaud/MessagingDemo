using Application.Events.Orders.Order_Created;
using Contracts.Order;
using MassTransit;
using MediatR;
using Serilog;

namespace Application.Consumers.Orders;

public class OrderCreatedConsumer(ILogger logger, IMediator mediator) : IConsumer<OrderCreated>
{
    private readonly ILogger _logger = logger;
    private readonly IMediator _mediator = mediator;

    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        _logger.Information(nameof(OrderCreatedConsumer));
        await _mediator.Publish(new OrderCreatedEvent(context.Message));
    }
}
