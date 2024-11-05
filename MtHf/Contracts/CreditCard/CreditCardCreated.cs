namespace Contracts.CreditCard;

public record CreditCardCreated(Guid Id, string HolderName, string CardType, string CardNumber, DateOnly ExpiryDate, string MvcCode, Guid CustomerId);
