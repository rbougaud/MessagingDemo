using Contracts.Customer;
using MediatR;

namespace Application.Events.Customer.CustomerAdded;

public readonly record struct CustomerCreatedEvent(CustomerCreated CustomerCreated) : INotification;
