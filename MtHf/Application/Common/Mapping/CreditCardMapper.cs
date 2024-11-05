using Application.Common.Dto.CreditCards;
using Contracts.CreditCard;
using Domain.Abstraction.CreditCards;
using Domain.Entities;
using Domain.Entities.Projections;

namespace Application.Common.Mapping;

public static class CreditCardMapper
{
    public static CreditCard ToDao(this CreditCardCreated creditCardCreated)
    {
        return new CreditCard
        {
            Id = creditCardCreated.Id,
            CardNumber = creditCardCreated.CardNumber,
            CardType = creditCardCreated.CardType,
            HolderName = creditCardCreated.HolderName,
            MvcCode = creditCardCreated.MvcCode,
            ExpiryDate = creditCardCreated.ExpiryDate,
            CustomerId = creditCardCreated.CustomerId
        };
    }

    public static CreditCardDto ToDto(this CreditCardProjection creditCard)
    {
        return new CreditCardDto
        {
            Id = creditCard.Id,
            CardNumber = creditCard.CardNumber,
            CardType = creditCard.CardType,
            HolderName = creditCard.HolderName,
            MvcCode = creditCard.MvcCode,
            ExpiryDate = creditCard.ExpiryDate,
            CustomerId = creditCard.CustomerId
        };
    }

    public static CreditCardProjection ToDao(this ICreditCardDto dto)
    {
        return new CreditCardProjection
        {
            Id = dto.Id,
            CardNumber = dto.CardNumber,
            CardType = dto.CardType,
            HolderName = dto.HolderName,
            MvcCode = dto.MvcCode,
            ExpiryDate = dto.ExpiryDate,
            CustomerId = dto.CustomerId
        };
    }
}
