using Application.Events.Customer.Customer_Deleted;
using Contracts.Customer;
using MassTransit;
using MediatR;
using Serilog;

namespace Application.Consumers.Customers;

public class CustomerDeletedConsumer(ILogger logger, IMediator mediator) : IConsumer<CustomerDeleted>
{
    private readonly ILogger _logger = logger;
    private readonly IMediator _mediator = mediator;

    public async Task Consume(ConsumeContext<CustomerDeleted> context)
    {
        _logger.Information(nameof(CustomerDeletedConsumer));
        await _mediator.Publish(new CustomerDeletedEvent(context.Message));
    }
}
