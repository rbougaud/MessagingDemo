using Domain.Common.Helper;
using MediatR;

namespace Application.Services.Orders.Commands.UpdateDeliveryOrder;

public readonly record struct UpdateDeliveryOrderCommand(Guid OrderId, short State, short DeliveryMode, DateOnly DeliveryDate) : IRequest<Result<UpdateDeliveryOrderResponse, List<string>>>;
