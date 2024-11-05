using Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.Events.CreditCards.CreditCard_Deleted;

public class CreditCardDeletedEventHandler(ILogger logger, WriterContext context) : INotificationHandler<CreditCardDeletedEvent>
{
    private readonly ILogger _logger = logger;
    private readonly WriterContext _context = context;

    public async Task Handle(CreditCardDeletedEvent @event, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(CreditCardDeletedEventHandler));
        try
        {
            var existingProjection = await _context.CreditCardProjections.AsNoTracking().FirstOrDefaultAsync(x => x.CustomerId == @event.CreditCardDeleted.CustomerId);
            if (existingProjection is not null)
            {
                _context.CreditCardProjections.Remove(existingProjection);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
        }
    }
}
