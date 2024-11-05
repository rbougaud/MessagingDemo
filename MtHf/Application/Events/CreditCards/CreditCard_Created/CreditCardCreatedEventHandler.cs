using Domain.Entities.Projections;
using Infrastructure.Persistence.Context;
using MediatR;
using Serilog;

namespace Application.Events.CreditCards.CreditCard_Created;

public class CreditCardCreatedEventHandler(ILogger logger, WriterContext context) : INotificationHandler<CreditCardCreatedEvent>
{
    private readonly ILogger _logger = logger;
    private readonly WriterContext _context = context;

    public async Task Handle(CreditCardCreatedEvent @event, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(CreditCardCreatedEventHandler));
        try
        {
            bool existingProjection = _context.CreditCardProjections.Any(x => x.CustomerId == @event.CreditCardCreated.CustomerId);
            if (!existingProjection)
            {
                var customer = _context.CustomerProjections.Find(@event.CreditCardCreated.CustomerId)!;

                var projection = new CreditCardProjection
                {
                    Id = @event.CreditCardCreated.Id,
                    CardNumber = @event.CreditCardCreated.CardNumber,
                    CardType = @event.CreditCardCreated.CardType,
                    HolderName = @event.CreditCardCreated.HolderName,
                    MvcCode = @event.CreditCardCreated.MvcCode,
                    ExpiryDate = @event.CreditCardCreated.ExpiryDate,
                    CustomerId = @event.CreditCardCreated.CustomerId,
                    Customer = customer
                };
                _context.CreditCardProjections.Add(projection);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
        }
    }
}
