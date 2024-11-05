namespace Domain.Abstraction.CreditCards;

public interface ICreditCardDto
{
    Guid Id { get; init; }
    string HolderName { get; init; }
    string CardType { get; init; }
    string CardNumber { get; init; }
    DateOnly ExpiryDate { get; init; }
    string MvcCode { get; init; }
    Guid CustomerId { get; init; }
}
