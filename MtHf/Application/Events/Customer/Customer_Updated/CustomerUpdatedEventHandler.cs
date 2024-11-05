using Application.Services.Mailing;
using Infrastructure.Persistence.Context;
using MediatR;
using Serilog;

namespace Application.Events.Customer.Customer_Updated;

public class CustomerUpdatedEventHandler(WriterContext context, ILogger logger) : INotificationHandler<CustomerUpdatedEvent>
{
    private readonly ILogger _logger = logger;
    private readonly WriterContext _context = context;

    public async Task Handle(CustomerUpdatedEvent @event, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(CustomerUpdatedEventHandler));
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var existingProjection = await _context.CustomerProjections.FindAsync(@event.CustomerUpdated.Id);
            if (existingProjection is not null)
            {
                existingProjection.Address = @event.CustomerUpdated.Address;
                existingProjection.Iban = @event.CustomerUpdated.Iban;
                existingProjection.Mail = @event.CustomerUpdated.Mail;
                existingProjection.Phone = @event.CustomerUpdated.Phone;

                _context.CustomerProjections.Update(existingProjection);
                await _context.SaveChangesAsync();

                var emailService = new EmailService(_logger, "smtp.example.com", 587, "votre_email@example.com", "votre_mot_de_passe");
                emailService.SendEmail(@event.CustomerUpdated.Mail, "Update infos Customer", "All your information have been updated");
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
