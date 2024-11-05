namespace Domain.Abstraction.LoyaltyAccounts;

public interface ILoyaltyAccountDto
{
     Guid LoyaltyAccountId { get; init; }
     int Points { get; init; }
     Guid CustomerId { get; init; }
}
