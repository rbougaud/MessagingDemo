using Domain.Common.Helper;
using MediatR;

namespace Application.Services.Customers.Commands.DeleteCustomerByFullName;

public record DeleteCustomerByFullNameCommand(Guid Id, string FirstName, string LastName) : IRequest<Result<DeleteCustomerByFullNameResponse, List<string>>>;
