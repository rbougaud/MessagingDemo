using Domain.Abstraction.LoyaltyAccounts;
using Domain.Common.Helper;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Persistence.Repositories;

public class LoyaltyAccountRepositoryReader(ILogger logger, ReaderContext context) : ILoyaltyAccountRepositoryReader
{
    private readonly ILogger _logger = logger;
    private readonly ReaderContext _context = context;

    public async Task<Result<LoyaltyAccount?, Exception>> GetByCustomerId(Guid customerId)
    {
        _logger.Information(nameof(GetByCustomerId));
        var existingAccount = await _context.LoyaltyAccounts.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.CustomerId == customerId);
        if (existingAccount is null)
        {
            return new Exception("There is no Loyalty Account for this customer");
        }
        return existingAccount;
    }
}
