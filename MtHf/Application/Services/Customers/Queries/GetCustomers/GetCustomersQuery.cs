using Domain.Common.Helper;
using MediatR;

namespace Application.Services.Customers.Queries.GetCustomers;

public record GetCustomersQuery() : IRequest<Result<GetCustomersResponse, string>>;
