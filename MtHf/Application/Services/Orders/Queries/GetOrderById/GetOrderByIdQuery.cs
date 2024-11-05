using Domain.Common.Helper;
using MediatR;

namespace Application.Services.Orders.Queries.GetOrderById;

public readonly record struct GetOrderByIdQuery(Guid OrderId) : IRequest<Result<GetOrderByIdResponse, string>>;
