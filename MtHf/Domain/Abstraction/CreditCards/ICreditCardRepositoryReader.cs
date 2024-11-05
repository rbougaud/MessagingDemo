using Domain.Common.Helper;

namespace Domain.Abstraction.CreditCards;

public interface ICreditCardRepositoryReader
{
    Result<bool, Exception> CheckIfExist(Guid customerId);
}
