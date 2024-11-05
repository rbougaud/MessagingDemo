using Contracts.Customer;
using MediatR;

namespace Application.Events.Customer.Customer_Updated;

public readonly record struct CustomerUpdatedEvent(CustomerUpdated CustomerUpdated) : INotification;
