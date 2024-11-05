using MediatR;

namespace Application.Events.LoyaltyAccounts.AddLoyaltyAccount;

public readonly record struct LoyaltyAccountCreatedEvent(Guid Id, int Points, Guid CustomerId) : INotification;
