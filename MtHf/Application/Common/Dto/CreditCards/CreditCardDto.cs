using Domain.Abstraction.CreditCards;

namespace Application.Common.Dto.CreditCards;

public record CreditCardDto : ICreditCardDto
{
    public Guid Id { get; init; }
    public required string HolderName { get; init; }
    public required string CardType { get; init; }
    public required string CardNumber { get; init; }
    public DateOnly ExpiryDate { get; init; }
    public required string MvcCode { get; init; }
    public Guid CustomerId { get; init; }
}
