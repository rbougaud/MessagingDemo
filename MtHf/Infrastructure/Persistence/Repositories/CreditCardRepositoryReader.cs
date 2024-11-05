using Domain.Abstraction.CreditCards;
using Domain.Common.Helper;
using Infrastructure.Persistence.Context;
using Serilog;

namespace Infrastructure.Persistence.Repositories;

public class CreditCardRepositoryReader(ILogger logger, ReaderContext context) : ICreditCardRepositoryReader
{
    private readonly ILogger _logger = logger;
    private readonly ReaderContext _context = context;

    public Result<bool, Exception> CheckIfExist(Guid customerId)
    {
        _logger.Information(nameof(CheckIfExist));
        bool hasExistingCreditCard = _context.CreditCards.Any(x => x.CustomerId == customerId);
        if (hasExistingCreditCard) { return new Exception("This customer has already a credit card"); }
        return hasExistingCreditCard;
    }
}
