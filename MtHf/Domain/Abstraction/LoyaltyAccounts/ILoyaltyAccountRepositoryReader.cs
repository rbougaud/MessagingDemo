using Domain.Common.Helper;
using Domain.Entities;

namespace Domain.Abstraction.LoyaltyAccounts;

public interface ILoyaltyAccountRepositoryReader
{
    Task<Result<LoyaltyAccount?, Exception>> GetByCustomerId(Guid customerId);
}
