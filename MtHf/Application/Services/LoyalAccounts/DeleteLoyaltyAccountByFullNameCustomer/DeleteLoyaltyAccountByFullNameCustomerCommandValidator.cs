using FluentValidation;

namespace Application.Services.LoyalAccounts.DeleteLoyaltyAccountByIdCustomer;

public class DeleteLoyaltyAccountByFullNameCustomerCommandValidator : AbstractValidator<DeleteLoyaltyAccountByFullNameCustomerCommand>
{
    public DeleteLoyaltyAccountByFullNameCustomerCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
    }
}
