using Domain.Common.Helper;
using MediatR;

namespace Application.Services.LoyalAccounts.DeleteLoyaltyAccountByIdCustomer;

public record DeleteLoyaltyAccountByFullNameCustomerCommand(string FirstName, string LastName) : IRequest<Result<DeleteLoyaltyAccountByFullNameCustomerResponse, List<string>>>;
