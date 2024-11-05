using Domain.Abstraction.LoyaltyAccounts;
using Domain.Common.Helper;
using Domain.Entities;
using Domain.Entities.Projections;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Persistence.Repositories;

public class LoyaltyAccountRepositoryWriter(ILogger logger, WriterContext context) : ILoyaltyAccountRepositoryWriter
{
    private readonly ILogger _logger = logger;
    private readonly WriterContext _context = context;
    
    public async Task<Result<Guid, Exception>> AddAsync(LoyaltyAccount account)
    {
        _logger.Information(nameof(AddAsync));
        _context.LoyaltyAccounts.Add(account);
        var result = await _context.SaveChangesAsync();
        return result > 0 ? account.Id : Guid.Empty;
    }

    public async Task<Result<bool, Exception>> DeleteByCustomerFullNameAsync(string firstName, string lastName)
    {
        _logger.Information(nameof(DeleteByCustomerFullNameAsync));
        var existingAccount = await _context.LoyaltyAccounts.AsNoTracking().FirstOrDefaultAsync(x => x.Customer.FirstName.Equals(firstName) && x.Customer.LastName.Equals(lastName));
        if (existingAccount is null)
        {
            return new Exception("This movie does not exist !");
        }

        // Implementation soft delete
        var isDeleted = await _context.LoyaltyAccounts
            .Where(x => x.Customer.FirstName.Equals(firstName) && x.Customer.LastName.Equals(lastName) && !x.IsDeleted)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsDeleted, true)
                .SetProperty(p => p.DeletedAtUtc, DateTime.UtcNow));

        return isDeleted > 0;
    }

    public async Task<Result<bool, Exception>> DeleteByCustomerIdAsync(Guid customerId)
    {
        _logger.Information(nameof(DeleteByCustomerIdAsync));

        var existingAccount = await _context.LoyaltyAccounts.AsNoTracking().FirstOrDefaultAsync(x => x.CustomerId == customerId);
        if (existingAccount is null)
        {
            return new Exception("This movie does not exist !");
        }
        // Implementation soft delete
        var isDeleted = await _context.LoyaltyAccounts
            .Where(x => x.CustomerId == customerId && !x.IsDeleted)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsDeleted, true)
                .SetProperty(p => p.DeletedAtUtc, DateTime.UtcNow));

        return isDeleted > 0;
    }

    public async Task<Result<bool, Exception>> UpdateAsync(LoyaltyAccount account)
    {
        _logger.Information(nameof(UpdateAsync));
        try
        {
            var existingAccount = await _context.LoyaltyAccounts.AsNoTracking().FirstOrDefaultAsync(x => x.CustomerId == account.CustomerId);
            if (existingAccount is null)
            {
                return new Exception("This Loyalty Account or the customer does not exist !");
            }
            existingAccount.Points = account.Points;

            _context.LoyaltyAccounts.Update(existingAccount);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ex;
        }
    }

    public async Task<Result<bool, Exception>> UndoDeletedAsync(LoyaltyAccount account)
    {
        _logger.Information(nameof(UpdateAsync));
        try
        {
            account.DeletedAtUtc = null;
            account.IsDeleted = false;

            _context.LoyaltyAccounts.Update(account);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ex;
        }
    }

    public async Task UpdatePointsAsync(LoyaltyAccountProjection loyaltyAccount)
    {
        _logger.Information(nameof(UpdatePointsAsync));
        _context.LoyaltyAccountProjections.Update(loyaltyAccount);
        var customer = _context.Customers.Find(loyaltyAccount.Id);
        await UpdateAsync(new LoyaltyAccount { Id = loyaltyAccount.Id, Points = loyaltyAccount.Points, CustomerId = loyaltyAccount.CustomerId, Customer = customer!});
    }
}
