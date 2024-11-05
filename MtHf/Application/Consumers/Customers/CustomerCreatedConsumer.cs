using Application.Events.Customer.CustomerAdded;
using Contracts.Customer;
using MassTransit;
using MediatR;
using Serilog;

namespace Application.Consumers.Customers;

public class CustomerCreatedConsumer(ILogger logger, IMediator mediator) : IConsumer<CustomerCreated>
{
    private readonly ILogger _logger = logger;
    private readonly IMediator _mediator = mediator;

    public async Task Consume(ConsumeContext<CustomerCreated> context)
    {
        _logger.Information(nameof(CustomerCreatedConsumer));
        await _mediator.Publish(new CustomerCreatedEvent(context.Message));
    }
}
