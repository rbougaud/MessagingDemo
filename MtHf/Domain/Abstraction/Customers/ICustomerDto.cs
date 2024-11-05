using Domain.Abstraction.CreditCards;
using Domain.Abstraction.LoyaltyAccounts;

namespace Domain.Abstraction.Customers;

public interface ICustomerDto
{
    Guid Id { get; init; }
    string FirstName { get; init; }
    string LastName { get; init; }
    string Mail { get; init; }
    string? Address { get; init; }
    string? Phone { get; init; }
    string? Iban { get; init; }
    ILoyaltyAccountDto? LoyaltyAccountDto { get; init; }
    ICreditCardDto? CreditCardDto { get; init; }
}
