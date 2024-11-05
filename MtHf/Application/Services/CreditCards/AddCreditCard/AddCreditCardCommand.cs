using Domain.Common.Helper;
using MediatR;

namespace Application.Services.CreditCards.AddCreditCard;

public readonly record struct AddCreditCardCommand(string HolderName, string CardType, string CardNumber, DateOnly ValidityDate, string MvcCode, Guid CustomerId) : IRequest<Result<AddCreditCardResponse, List<string>>>;
