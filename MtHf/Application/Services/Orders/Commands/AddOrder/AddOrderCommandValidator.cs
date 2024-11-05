using FluentValidation;

namespace Application.Services.Orders.Commands.AddOrder;

public class AddOrderCommandValidator : AbstractValidator<AddOrderCommand>
{
    public AddOrderCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required.");
        RuleFor(x => x.Mail).NotEmpty().WithMessage("Mail is required.");
    }
}
