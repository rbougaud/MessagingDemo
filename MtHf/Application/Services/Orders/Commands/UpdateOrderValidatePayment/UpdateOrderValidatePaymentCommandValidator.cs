using FluentValidation;

namespace Application.Services.Orders.Commands.UpdateOrderValidatePayment;

public class UpdateOrderValidatePaymentCommandValidator : AbstractValidator<UpdateOrderValidatePaymentCommand>
{
    public UpdateOrderValidatePaymentCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty().WithMessage("Id is required.");
    }
}
