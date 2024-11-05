using Domain.Common.Helper;
using MediatR;

namespace Application.Services.Orders.Queries.GetOrders;

public record GetOrdersQuery() : IRequest<Result<GetOrdersResponse, string>>;
