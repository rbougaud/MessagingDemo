using Domain.Common.Helper;
using MediatR;

namespace Application.Services.Orders.Commands.DeleteOrderById;

public readonly record struct DeleteOrderByIdCommand(Guid OrderId) : IRequest<Result<DeleteOrderByIdResponse, List<string>>>;
