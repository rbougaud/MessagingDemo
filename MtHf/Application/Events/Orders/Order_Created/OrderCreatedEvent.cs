using Contracts.Order;
using MediatR;

namespace Application.Events.Orders.Order_Created;

public readonly record struct OrderCreatedEvent(OrderCreated OrderCreated) : INotification;
