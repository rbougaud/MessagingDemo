using Application.Events.LoyaltyAccounts.DeletedLoyaltyAccountByCustomerId;
using Application.Services.Mailing;
using Contracts.LoyaltyAccount;
using Domain.Abstraction.Customers;
using MassTransit;
using MediatR;
using Serilog;
namespace Application.Consumers.LoyaltyAccounts;

public class LoyaltyAccountDeletedConsumer(ILogger logger, IMediator mediator, ICustomerRepositoryReader customerRepositoryReader) : IConsumer<LoyaltyAccountDeleted>
{
    private readonly ILogger _logger = logger;
    private readonly IMediator _mediator = mediator;
    private readonly ICustomerRepositoryReader _customerRepositoryReader = customerRepositoryReader;

    public async Task Consume(ConsumeContext<LoyaltyAccountDeleted> context)
    {
        _logger.Information(nameof(LoyaltyAccountDeletedConsumer));
        await _mediator.Publish(new LoyaltyAccountDeletedEvent(context.Message.FirstName, context.Message.LastName));

        var customerResult = await _customerRepositoryReader.GetCustomerByFullNameAsync(context.Message.FirstName, context.Message.LastName);
        if (customerResult.IsSuccess && customerResult.Value != null)
        {
            var emailService = new EmailService(_logger, "smtp.example.com", 587, "votre_email@example.com", "votre_mot_de_passe");
            emailService.SendEmail(customerResult.Value.Mail, "Loyalty account Removed", $"Your loyalty account has been deleted.");
        }
    }
}
