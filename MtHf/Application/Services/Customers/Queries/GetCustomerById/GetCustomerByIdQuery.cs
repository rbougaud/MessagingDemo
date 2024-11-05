using Domain.Common.Helper;
using MediatR;

namespace Application.Services.Customers.Queries.GetCustomerById;

public readonly record struct GetCustomerByIdQuery(Guid Id) : IRequest<Result<GetCustomerByIdResponse, string>>;
