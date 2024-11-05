using Contracts.Order;
using MediatR;

namespace Application.Events.Orders.Order_PaymentValidated;

public readonly record struct OrderPaymentValidatedEvent(OrderPaymentValidated OrderPaymentValidated) : INotification;
