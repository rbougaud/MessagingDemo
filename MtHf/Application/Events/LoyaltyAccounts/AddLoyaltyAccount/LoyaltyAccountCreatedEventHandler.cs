using Domain.Entities.Projections;
using Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.Events.LoyaltyAccounts.AddLoyaltyAccount;

public class LoyaltyAccountCreatedEventHandler(ILogger logger, WriterContext context) : INotificationHandler<LoyaltyAccountCreatedEvent>
{
    private readonly ILogger _logger = logger;
    private readonly WriterContext _context = context;

    public async Task Handle(LoyaltyAccountCreatedEvent @event, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(LoyaltyAccountCreatedEventHandler));
        try
        {
            var existingProjection = await _context.LoyaltyAccountProjections.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == @event.Id);
            if (existingProjection is null)
            {
                var customer = _context.CustomerProjections.Find(@event.CustomerId)!;

                var projection = new LoyaltyAccountProjection
                {
                    Id = @event.Id,
                    Points = @event.Points,
                    CustomerId = @event.CustomerId,
                    Customer = customer,
                };
                _context.LoyaltyAccountProjections.Add(projection);
                await _context.SaveChangesAsync(cancellationToken);
            }
            else if(existingProjection.IsDeleted) 
            {
                existingProjection.Points = @event.Points;
                existingProjection.IsDeleted = false;
                existingProjection.DeletedAtUtc = null;

                _context.LoyaltyAccountProjections.Update(existingProjection);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
        }
    }
}
