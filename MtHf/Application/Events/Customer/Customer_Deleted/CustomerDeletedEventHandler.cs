using Application.Services.Mailing;
using Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.Events.Customer.Customer_Deleted;

public class CustomerDeletedEventHandler(WriterContext context, ILogger logger) : INotificationHandler<CustomerDeletedEvent>
{
    private readonly ILogger _logger = logger;
    private readonly WriterContext _context = context;

    public async Task Handle(CustomerDeletedEvent @event, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(CustomerDeletedEventHandler));
        using var transaction = await _context.Database.BeginTransactionAsync();
        string? email = null;
        try
        {
            var existingProjections = await _context.CustomerProjections.Where(x => x.FirstName.Equals(@event.CustomerDeleted.FirstName)
                                                                                 && x.LastName.Equals(@event.CustomerDeleted.LastName)).ToListAsync();
            if (existingProjections is not null && existingProjections.Count != 0)
            {
                email = existingProjections[^1].Mail;
                _context.CustomerProjections.RemoveRange(existingProjections);
                await _context.SaveChangesAsync();

                _logger.Information("Email Customer to delete : {email}", email);
            }
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.Error(ex.Message);
        }

        if (email is not null)
        {
            var emailService = new EmailService(_logger, "smtp.example.com", 587, "votre_email@example.com", "votre_mot_de_passe");
            emailService.SendEmail(email, "Remove Account", "Your account has been deleted");
        }
    }
}
