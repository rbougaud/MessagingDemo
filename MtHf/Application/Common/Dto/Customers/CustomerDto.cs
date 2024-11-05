using Domain.Abstraction.CreditCards;
using Domain.Abstraction.Customers;
using Domain.Abstraction.LoyaltyAccounts;

namespace Application.Common.Dto.Customers;

public class CustomerDto : ICustomerDto
{
    public Guid Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Mail { get; init; }
    public string? Address { get; init; }
    public string? Phone { get; init; }
    public string? Iban { get; init; }
    public ILoyaltyAccountDto? LoyaltyAccountDto { get; init; }
    public ICreditCardDto? CreditCardDto { get; init; }
}
