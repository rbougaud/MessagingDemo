using Application.Events.Customer.Customer_Updated;
using Contracts.Customer;
using MassTransit;
using MediatR;
using Serilog;

namespace Application.Consumers.Customers;

public class CustomerUpdatedConsumer(ILogger logger, IMediator mediator) : IConsumer<CustomerUpdated>
{
    private readonly ILogger _logger = logger;
    private readonly IMediator _mediator = mediator;

    public async Task Consume(ConsumeContext<CustomerUpdated> context)
    {
        _logger.Information(nameof(CustomerUpdatedConsumer));
        await _mediator.Publish(new CustomerUpdatedEvent(context.Message));
    }
}
