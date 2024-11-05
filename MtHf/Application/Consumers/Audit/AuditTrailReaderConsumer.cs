using Contracts.Audit;
using Infrastructure.Persistence.Context;
using MassTransit;

namespace Application.Consumers.Audit;

public class AuditTrailReaderConsumer(WriterContext context) : IConsumer<AuditTrailReaderMessage>
{
    private readonly WriterContext _context = context;

    public async Task Consume(ConsumeContext<AuditTrailReaderMessage> context)
    {
        _context.AuditReadEntries.Add(context.Message.AuditReadEntry);
        await _context.SaveChangesAsync();
    }
}
