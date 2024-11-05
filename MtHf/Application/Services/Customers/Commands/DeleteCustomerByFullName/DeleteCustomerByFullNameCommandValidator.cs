using FluentValidation;

namespace Application.Services.Customers.Commands.DeleteCustomerByFullName;

public class DeleteCustomerByFullNameCommandValidator : AbstractValidator<DeleteCustomerByFullNameCommand>
{
    public DeleteCustomerByFullNameCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required.");
    }
}
