using Domain.Common.Helper;
using MediatR;

namespace Application.Services.Customers.Commands.AddCustomer;

public record AddCustomerCommand(string FirstName, string LastName, string Mail, string? Address, string? Phone, string? Iban) : IRequest<Result<AddCustomerResponse, List<string>>>;
