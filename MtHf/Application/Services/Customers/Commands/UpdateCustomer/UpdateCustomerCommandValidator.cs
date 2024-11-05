using FluentValidation;

namespace Application.Services.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandValidator  : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required.");
        RuleFor(x => x.Mail).NotEmpty().WithMessage("Mail is required.");
    }
}
