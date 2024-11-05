using MediatR;

namespace Application.Events.LoyaltyAccounts.DeletedLoyaltyAccountByCustomerId;

public readonly record struct LoyaltyAccountDeletedEvent(string FirstName, string LastName) : INotification;
