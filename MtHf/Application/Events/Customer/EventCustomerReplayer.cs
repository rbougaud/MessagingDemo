using Application.Events.Customer.Customer_Deleted;
using Application.Events.Customer.Customer_Updated;
using Application.Events.Customer.CustomerAdded;
using Contracts.Customer;
using Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;

namespace Application.Events.Customer;

public class EventCustomerReplayer(ReaderContext dbContext, IMediator mediator, ILogger logger)
{
    private readonly ILogger _logger = logger;
    private readonly IMediator _mediator = mediator;
    private readonly ReaderContext _dbContext = dbContext;

    public async Task ReplayEventsAsync()
    {
        _logger.Information(nameof(ReplayEventsAsync));
        var events = await _dbContext.EventStore.OrderBy(e => e.CreatedAt).ToListAsync();

        foreach (var storedEvent in events)
        {
            var eventType = Type.GetType(storedEvent.EventType);
            var @event = JsonConvert.DeserializeObject(storedEvent.Data, eventType);

            if (@event is CustomerCreated customerCreatedEvent)
            {
                await _mediator.Publish(new CustomerCreatedEvent(customerCreatedEvent));
            }
            else if (@event is CustomerUpdated customerUpdatedEvent)
            {
                await _mediator.Publish(new CustomerUpdatedEvent(customerUpdatedEvent));
            }
            else if (@event is CustomerDeleted customerDeletedEvent)
            {
                await _mediator.Publish(new CustomerDeletedEvent(customerDeletedEvent));
            }
        }
    }
}
