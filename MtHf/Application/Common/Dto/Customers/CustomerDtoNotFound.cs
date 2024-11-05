using Domain.Abstraction.CreditCards;
using Domain.Abstraction.Customers;
using Domain.Abstraction.LoyaltyAccounts;

namespace Application.Common.Dto.Customers;

public class CustomerDtoNotFound : ICustomerDto
{
    public Guid Id { get; init; } = Guid.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Mail { get; init; } = string.Empty;
    public string? Address { get; init; }
    public string? Phone { get; init; }
    public string? Iban { get; init; }
    public ILoyaltyAccountDto? LoyaltyAccountDto { get; init; }
    public ICreditCardDto? CreditCardDto { get; init; }
}
