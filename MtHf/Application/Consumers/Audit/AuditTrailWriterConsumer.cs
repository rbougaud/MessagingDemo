using Contracts.Audit;
using Infrastructure.Persistence.Context;
using MassTransit;

namespace Application.Consumers.Audit;

public class AuditTrailWriterConsumer(WriterContext context) : IConsumer<AuditTrailWriterMessage>
{
    private readonly WriterContext _context = context;

    public async Task Consume(ConsumeContext<AuditTrailWriterMessage> context)
    {
        _context.AuditEntries.AddRange(context.Message.AuditEntries);
        await _context.SaveChangesAsync();
    }
}
