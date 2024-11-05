using FluentValidation;

namespace Application.Services.CreditCards.DeleteCreditCardByIdCustomer;

public class DeleteCreditCardByIdCustomerCommandValidator : AbstractValidator<DeleteCreditCardByIdCustomerCommand>
{
    public DeleteCreditCardByIdCustomerCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
    }
}
