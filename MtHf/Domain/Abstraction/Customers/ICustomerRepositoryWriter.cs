using Domain.Common.Helper;
using Domain.Entities;

namespace Domain.Abstraction.Customers;

public interface ICustomerRepositoryWriter
{
    Task<Result<Guid, Exception>> AddAsync(Customer customer);
    Result<bool, Exception> CheckCustomerExist(string firstName, string lastName);
    Task<Result<bool, Exception>> DeleteAsync(string firstName, string lastName);
    Task<Result<bool, Exception>> UpdateAsync(Customer customer);
}
