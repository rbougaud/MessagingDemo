using Domain.Common.Helper;
using Domain.Entities.Projections;

namespace Domain.Abstraction.Customers;

public interface ICustomerRepositoryReader
{
    Task<Result<CustomerProjection?, Exception>> GetCustomerByIdAsync(Guid id);
    Task<List<string>> GetAllMailsAsync();
    Task<Result<CustomerProjection?, Exception>> GetCustomerByFullNameAsync(string firstName, string lastName);
    Task<Result<IEnumerable<CustomerProjection>, Exception>> GetAllAsync();
}
