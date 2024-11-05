using Contracts.CreditCard;
using MediatR;

namespace Application.Events.CreditCards.CreditCard_Deleted;

public readonly record struct CreditCardDeletedEvent(CreditCardDeleted CreditCardDeleted) : INotification;
