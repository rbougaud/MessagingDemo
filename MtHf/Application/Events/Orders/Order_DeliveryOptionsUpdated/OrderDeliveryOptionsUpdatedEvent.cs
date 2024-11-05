using Contracts.Order;
using MediatR;

namespace Application.Events.Orders.Order_DeliveryOptionsUpdated;

public readonly record struct OrderDeliveryOptionsUpdatedEvent(OrderDeliveryOptionsUpdated OrderDeliveryOptionsUpdated) : INotification;
