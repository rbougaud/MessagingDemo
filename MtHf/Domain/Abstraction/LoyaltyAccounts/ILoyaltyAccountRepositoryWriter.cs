using Domain.Common.Helper;
using Domain.Entities;
using Domain.Entities.Projections;

namespace Domain.Abstraction.LoyaltyAccounts;

public interface ILoyaltyAccountRepositoryWriter
{
    Task<Result<Guid, Exception>> AddAsync(LoyaltyAccount account);
    Task<Result<bool, Exception>> DeleteByCustomerIdAsync(Guid customerId);
    Task<Result<bool, Exception>> DeleteByCustomerFullNameAsync(string firstName, string lastName);
    Task<Result<bool, Exception>> UpdateAsync(LoyaltyAccount account);
    Task UpdatePointsAsync(LoyaltyAccountProjection loyaltyAccount);
    Task<Result<bool, Exception>> UndoDeletedAsync(LoyaltyAccount account);
}
