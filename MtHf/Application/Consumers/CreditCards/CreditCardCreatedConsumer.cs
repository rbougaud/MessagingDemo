using Application.Events.CreditCards.CreditCard_Created;
using Application.Services.Mailing;
using Contracts.CreditCard;
using Domain.Abstraction.Customers;
using MassTransit;
using MediatR;
using Serilog;

namespace Application.Consumers.CreditCards;

public class CreditCardCreatedConsumer(ILogger logger, IMediator mediator, ICustomerRepositoryReader customerRepositoryReader) : IConsumer<CreditCardCreated>
{
    private readonly ILogger _logger = logger;
    private readonly IMediator _mediator = mediator;
    private readonly ICustomerRepositoryReader _customerRepositoryReader = customerRepositoryReader;

    public async Task Consume(ConsumeContext<CreditCardCreated> context)
    {
        _logger.Information(nameof(CreditCardCreatedConsumer));
        await _mediator.Publish(new CreditCardCreatedEvent(context.Message));

        var customerResult = await _customerRepositoryReader.GetCustomerByIdAsync(context.Message.Id);
        if (customerResult.IsSuccess && customerResult.Value != null)
        {
            var emailService = new EmailService(_logger, "smtp.example.com", 587, "votre_email@example.com", "votre_mot_de_passe");
            emailService.SendEmail(customerResult.Value.Mail, "Regiter credit card", $"Your credit card has been saved for your next purchases.");
        }
    }
}
