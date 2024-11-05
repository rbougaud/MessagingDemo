using Domain.Common.Helper;
using Domain.Entities;

namespace Domain.Abstraction.CreditCards;

public interface ICreditCardRepositoryWriter
{
    Task<Result<Guid, Exception>> AddAsync(CreditCard creditCard);
    Task<Result<bool, Exception>> DeleteByCustomerIdAsync(Guid customerId);
}
