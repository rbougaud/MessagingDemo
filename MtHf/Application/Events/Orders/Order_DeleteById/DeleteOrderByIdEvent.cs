using MediatR;

namespace Application.Events.Orders.Order_DeleteById;

public readonly record struct DeleteOrderByIdEvent(Guid OrderId) : INotification;
