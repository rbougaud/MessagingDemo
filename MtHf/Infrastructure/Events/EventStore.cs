using Domain.Abstraction;
using Domain.Common.Helper;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Newtonsoft.Json;
using Serilog;

namespace Infrastructure.Events;

public class EventStore(ILogger logger, WriterContext context) : IEventStore
{
    private readonly WriterContext _context = context;
    private readonly ILogger _logger = logger;

    public async Task<Result<bool, Exception>> SaveAsync<T>(T @event, CancellationToken cancellationToken) where T : class
    {
        try
        {
            string qualifiedName = @event.GetType().AssemblyQualifiedName!;
            var data = JsonConvert.SerializeObject(@event);
            _logger.Information("Save {eventType}", qualifiedName);
            var eventStoreEntry = new EventStoreEntry
            {
                Id = Ulid.NewUlid().ToGuid(),
                AggregateId = ((dynamic)@event).Id.ToString(),
                EventType = qualifiedName,
                Data = data,
                CreatedAt = DateTimeOffset.UtcNow
            };

            await _context.EventStore.AddAsync(eventStoreEntry, cancellationToken);

            var outboxMessage = new OutboxMessage
            {
                Id = Ulid.NewUlid().ToGuid(),
                Type = qualifiedName,
                Payload = data,
                OccurredOn = DateTime.UtcNow,
                Processed = false
            };

            await _context.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
            int result = await _context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

}

