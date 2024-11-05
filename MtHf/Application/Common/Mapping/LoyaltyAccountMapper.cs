using Application.Common.Dto.LoyaltyAccounts;
using Contracts.LoyalAccount;
using Domain.Abstraction.LoyaltyAccounts;
using Domain.Entities;
using Domain.Entities.Projections;

namespace Application.Common.Mapping;

public static class LoyaltyAccountMapper
{
    public static LoyaltyAccount ToDao(this LoyaltyAccountCreated accountCreated)
    {
        return new LoyaltyAccount
        {
            Id = accountCreated.Id,
            Points = accountCreated.Points,
            CustomerId = accountCreated.CustomerId,
            IsDeleted = false,
            DeletedAtUtc = null
        };
    }

    public static LoyaltyAccountDto ToDto(this LoyaltyAccountProjection account)
    {
        return new LoyaltyAccountDto
        {
            LoyaltyAccountId = account.Id,
            Points = account.Points,
            CustomerId = account.CustomerId
        };
    }

    public static LoyaltyAccountProjection ToDao(this ILoyaltyAccountDto dto)
    {
        return new LoyaltyAccountProjection
        {
            Id = dto.LoyaltyAccountId,
            Points = dto.Points,
            CustomerId = dto.CustomerId
        };
    }
}
