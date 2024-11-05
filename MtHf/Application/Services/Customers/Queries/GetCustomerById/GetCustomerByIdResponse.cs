using Domain.Abstraction.Customers;

namespace Application.Services.Customers.Queries.GetCustomerById;

public record GetCustomerByIdResponse(ICustomerDto CustomerDto);
