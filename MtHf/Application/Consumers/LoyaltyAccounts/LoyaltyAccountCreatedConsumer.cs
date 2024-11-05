using Application.Events.LoyaltyAccounts.AddLoyaltyAccount;
using Application.Services.Mailing;
using Contracts.LoyalAccount;
using Domain.Abstraction.Customers;
using MassTransit;
using MediatR;
using Serilog;

namespace Application.Consumers.LoyaltyAccounts;

public class LoyaltyAccountCreatedConsumer(ILogger logger, IMediator mediator, ICustomerRepositoryReader customerRepositoryReader) : IConsumer<LoyaltyAccountCreated>
{
    private readonly ILogger _logger = logger;
    private readonly IMediator _mediator = mediator;
    private readonly ICustomerRepositoryReader _customerRepositoryReader = customerRepositoryReader;

    public async Task Consume(ConsumeContext<LoyaltyAccountCreated> context)
    {
        _logger.Information(nameof(LoyaltyAccountCreatedConsumer));

        await _mediator.Publish(new LoyaltyAccountCreatedEvent(context.Message.Id, context.Message.Points, context.Message.CustomerId));

        var customerResult = await _customerRepositoryReader.GetCustomerByIdAsync(context.Message.CustomerId);
        if (customerResult.IsSuccess && customerResult.Value != null)
        {
            var emailService = new EmailService(_logger, "smtp.example.com", 587, "votre_email@example.com", "votre_mot_de_passe");
            emailService.SendEmail(customerResult.Value.Mail, "New loyalty account", $"Your loyalty account has been created.");
        }
    }
}
