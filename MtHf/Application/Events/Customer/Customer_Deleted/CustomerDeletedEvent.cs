using Contracts.Customer;
using MediatR;

namespace Application.Events.Customer.Customer_Deleted;

public readonly record struct CustomerDeletedEvent(CustomerDeleted CustomerDeleted) : INotification;
