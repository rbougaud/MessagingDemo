using Application.Events.Orders.Order_DeleteById;
using Contracts.Order;
using MassTransit;
using MediatR;
using Serilog;

namespace Application.Consumers.Orders;

public class DeleteOrderByIdConsumer(ILogger logger, IMediator mediator) : IConsumer<OrderDeleted>
{
    private readonly ILogger _logger = logger;
    private readonly IMediator _mediator = mediator;

    public async Task Consume(ConsumeContext<OrderDeleted> context)
    {
        _logger.Information(nameof(Consume));
        await _mediator.Publish(new DeleteOrderByIdEvent(context.Message.Id));
    }
}
