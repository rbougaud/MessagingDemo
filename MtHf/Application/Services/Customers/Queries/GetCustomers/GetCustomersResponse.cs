using Application.Common.Dto.Customers;

namespace Application.Services.Customers.Queries.GetCustomers;

public record GetCustomersResponse(IReadOnlyList<CustomerDto> Customers);
