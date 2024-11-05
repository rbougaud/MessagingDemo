using Domain.Entities.Projections;
using Infrastructure.Persistence.Context;
using MediatR;
using Serilog;

namespace Application.Events.Customer.CustomerAdded;

public class CustomerCreatedEventHandler(WriterContext dbContext, ILogger logger) : INotificationHandler<CustomerCreatedEvent>
{
    private readonly WriterContext _context = dbContext;
    private readonly ILogger _logger = logger;

    public async Task Handle(CustomerCreatedEvent @event, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(CustomerCreatedEventHandler));
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var existingProjection = await _context.CustomerProjections.FindAsync(@event.CustomerCreated.Id);
            if (existingProjection == null)
            {
                var projection = new CustomerProjection
                {
                    Id = @event.CustomerCreated.Id,
                    FirstName = @event.CustomerCreated.FirstName,
                    LastName = @event.CustomerCreated.LastName,
                    Mail = @event.CustomerCreated.Mail,
                    Address = @event.CustomerCreated.Address,
                    Phone = @event.CustomerCreated.Phone,
                    Iban = @event.CustomerCreated.Iban
                };
                _context.CustomerProjections.Add(projection);
                await _context.SaveChangesAsync();
            }
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.Error(ex.Message);
        }
    }
}

