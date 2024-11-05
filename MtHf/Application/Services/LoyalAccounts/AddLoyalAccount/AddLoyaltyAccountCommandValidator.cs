using FluentValidation;

namespace Application.Services.LoyalAccounts.AddLoyalAccount;

public class AddLoyaltyAccountCommandValidator : AbstractValidator<AddLoyaltyAccountCommand>
{
    public AddLoyaltyAccountCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
    }
}
