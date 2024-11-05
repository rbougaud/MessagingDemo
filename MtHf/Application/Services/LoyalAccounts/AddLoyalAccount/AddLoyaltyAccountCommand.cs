using Domain.Common.Helper;
using MediatR;

namespace Application.Services.LoyalAccounts.AddLoyalAccount;

public record AddLoyaltyAccountCommand(string FirstName, string LastName) : IRequest<Result<AddLoyaltyAccountResponse, List<string>>>;
