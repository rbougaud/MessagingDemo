using Contracts.CreditCard;
using MediatR;

namespace Application.Events.CreditCards.CreditCard_Created;

public readonly record struct CreditCardCreatedEvent(CreditCardCreated CreditCardCreated) : INotification;
