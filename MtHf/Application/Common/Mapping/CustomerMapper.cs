using Application.Common.Dto.Customers;
using Contracts.Customer;
using Domain.Abstraction.Customers;
using Domain.Entities;
using Domain.Entities.Projections;

namespace Application.Common.Mapping;

public static class CustomerMapper
{
    public static Customer ToDao(this CustomerCreated customerCreated)
    {
        return new Customer
        {
            Id = customerCreated.Id,
            FirstName = customerCreated.FirstName,
            LastName = customerCreated.LastName,
            Mail = customerCreated.Mail,
            Address = customerCreated.Address,
            Phone = customerCreated.Phone,
            Iban = customerCreated.Iban
        };
    }

    public static Customer ToDao(this CustomerUpdated customerCreated)
    {
        return new Customer
        {
            Id = customerCreated.Id,
            FirstName = customerCreated.FirstName,
            LastName = customerCreated.LastName,
            Mail = customerCreated.Mail,
            Address = customerCreated.Address,
            Phone = customerCreated.Phone,
            Iban = customerCreated.Iban
        };
    }

    public static CustomerProjection ToDao(this ICustomerDto dto)
    {
        return new CustomerProjection
        {
            Id = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Mail = dto.Mail,
            Address = dto.Address,
            Phone = dto.Phone,
            Iban = dto.Iban,
            CreditCard = dto.CreditCardDto?.ToDao(),
            LoyaltyAccount = dto.LoyaltyAccountDto?.ToDao(),
        };
    }

    public static ICustomerDto ToDto(this CustomerProjection customer)
    {
        if (customer is null) { return new CustomerDtoNotFound(); }
        return new CustomerDto
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Mail = customer.Mail,
            Address = customer.Address,
            Phone = customer.Phone,
            Iban = customer.Iban,
            LoyaltyAccountDto = customer.LoyaltyAccount?.ToDto(),
            CreditCardDto = customer.CreditCard?.ToDto()
        };
    }

}
