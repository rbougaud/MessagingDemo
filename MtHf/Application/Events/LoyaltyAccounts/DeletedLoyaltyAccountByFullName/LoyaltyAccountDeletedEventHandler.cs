using Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.Events.LoyaltyAccounts.DeletedLoyaltyAccountByCustomerId;

public class LoyaltyAccountDeletedEventHandler(ILogger logger, WriterContext context) : INotificationHandler<LoyaltyAccountDeletedEvent>
{
    private readonly ILogger _logger = logger;
    private readonly WriterContext _context = context;

    public async Task Handle(LoyaltyAccountDeletedEvent @event, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(LoyaltyAccountDeletedEventHandler));
        try
        {
            var existingProjection = await _context.LoyaltyAccountProjections.AsNoTracking().FirstOrDefaultAsync(x => x.Customer.FirstName.Equals(@event.FirstName) && x.Customer.LastName.Equals(@event.LastName));
            if (existingProjection is not null)
            {
                // Implementation soft delete
                var isDeleted = await _context.LoyaltyAccountProjections
                    .Where(x => x.Customer.FirstName.Equals(@event.FirstName) && x.Customer.LastName.Equals(@event.LastName) && !x.IsDeleted)
                    .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsDeleted, true)
                        .SetProperty(p => p.DeletedAtUtc, DateTime.UtcNow));
                if (isDeleted > 0)
                {
                    _logger.Information("The loyaltyAccount has been deleted correctly !");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
        }
    }
}
