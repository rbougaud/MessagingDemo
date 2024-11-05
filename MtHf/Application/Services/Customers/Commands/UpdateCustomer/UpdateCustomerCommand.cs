using Domain.Common.Helper;
using MediatR;

namespace Application.Services.Customers.Commands.UpdateCustomer;

public record UpdateCustomerCommand(Guid Id, string FirstName, string LastName, string Mail, string? Address, string? Phone, string? Iban) : IRequest<Result<UpdateCustomerResponse, List<string>>>;

