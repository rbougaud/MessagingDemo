using Application.Events.Orders.Order_PaymentValidated;
using Contracts.Order;
using MassTransit;
using MediatR;
using Serilog;

namespace Application.Consumers.Orders;

public class OrderPaymentValidatedConsumer(ILogger logger, IMediator mediator) : IConsumer<OrderPaymentValidated>
{
    private readonly ILogger _logger = logger;
    private readonly IMediator _mediator = mediator;

    public async Task Consume(ConsumeContext<OrderPaymentValidated> context)
    {
        _logger.Information(nameof(Consume));
        await _mediator.Publish(new OrderPaymentValidatedEvent(context.Message));
    }
}
