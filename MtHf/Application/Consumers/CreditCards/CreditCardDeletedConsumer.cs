using Application.Events.CreditCards.CreditCard_Deleted;
using Application.Services.Mailing;
using Contracts.CreditCard;
using Domain.Abstraction.Customers;
using MassTransit;
using MediatR;
using Serilog;

namespace Application.Consumers.CreditCards;

public sealed class CreditCardDeletedConsumer(ILogger logger, IMediator mediator, ICustomerRepositoryReader customerRepositoryReader) : IConsumer<CreditCardDeleted>
{
    private readonly ILogger _logger = logger;
    private readonly IMediator _mediator = mediator;
    private readonly ICustomerRepositoryReader _customerRepositoryReader = customerRepositoryReader;

    public async Task Consume(ConsumeContext<CreditCardDeleted> context)
    {
        _logger.Information(nameof(CreditCardDeletedConsumer));
        await _mediator.Publish(new CreditCardDeletedEvent(context.Message));

        var customerResult = await _customerRepositoryReader.GetCustomerByIdAsync(context.Message.CustomerId);
        if (customerResult.IsSuccess && customerResult.Value != null)
        {
            var emailService = new EmailService(_logger, "smtp.example.com", 587, "votre_email@example.com", "votre_mot_de_passe");
            emailService.SendEmail(customerResult.Value.Mail, "Credit card Removed", $"Your credit card has been deleted.");
        }
    }
}
