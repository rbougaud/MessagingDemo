using FluentValidation;

namespace Application.Services.CreditCards.AddCreditCard;

public class AddCreditCardCommandValidator : AbstractValidator<AddCreditCardCommand>
{
    public AddCreditCardCommandValidator()
    {
        RuleFor(x => x.HolderName).NotEmpty();
        RuleFor(x => x.CardNumber).NotEmpty();
        RuleFor(x => x.CardType).NotEmpty();
        RuleFor(x => x.MvcCode).NotEmpty().Length(3);
        RuleFor(x => x.ValidityDate).GreaterThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Validity date must be in the future.");
        RuleFor(x => x.CustomerId).NotEmpty();
    }
}
