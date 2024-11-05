using Domain.Abstraction;
using Hangfire;
using Infrastructure.Persistence.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;

namespace Infrastructure.Outbox;

public class OutboxProcessor(ILogger logger, ReaderContext context, IPublishEndpoint publishEndpoint) : IProcessOutboxMessagesJob
{
    private readonly ILogger _logger = logger;
    private readonly ReaderContext _context = context;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    [AutomaticRetry(Attempts = 3)]
    public async Task ProcessAsync()
    {
        var messages = await _context.OutboxMessages.Where(m => !m.Processed).OrderBy(x => x.OccurredOn).ToListAsync();
        _logger.Information("{name} - Nb messages à trairer : {count}", nameof(OutboxProcessor), messages.Count);
        foreach (var message in messages)
        {
            var eventType = Type.GetType(message.Type);
            if (eventType is null)
            {
                _logger.Error("Erreur type : {type}", message.Type);
            }
            else
            {
                _logger.Information("eventType : {eventType}", eventType);
                var @event = JsonConvert.DeserializeObject(message.Payload, eventType);
                await _publishEndpoint.Publish(@event);
                message.Processed = true;
            }
        }

        await _context.SaveChangesAsync();
    }
}
