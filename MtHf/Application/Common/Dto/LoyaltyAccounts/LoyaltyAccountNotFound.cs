using Domain.Abstraction.LoyaltyAccounts;

namespace Application.Common.Dto.LoyaltyAccounts;

public class LoyaltyAccountNotFound : ILoyaltyAccountDto
{
    public Guid LoyaltyAccountId { get ; init ; }
    public int Points { get ; init ; }
    public Guid CustomerId { get ; init ; }
}
