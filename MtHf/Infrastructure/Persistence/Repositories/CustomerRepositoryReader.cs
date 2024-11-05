using Domain.Abstraction.Customers;
using Domain.Common.Helper;
using Domain.Entities.Projections;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Persistence.Repositories;

public class CustomerRepositoryReader(ILogger logger, ReaderContext context) : ICustomerRepositoryReader
{
    private readonly ILogger _logger = logger;
    private readonly ReaderContext _context = context;

    public async Task<Result<IEnumerable<CustomerProjection>, Exception>> GetAllAsync()
    {
        _logger.Information(nameof(GetAllAsync));
        return await _context.CustomerProjections.Include(x => x.LoyaltyAccount).ToListAsync();
    }

    public async Task<List<string>> GetAllMailsAsync()
    {
        _logger.Information(nameof(GetAllMailsAsync));
        return await _context.CustomerProjections.Select(x => x.Mail).ToListAsync();
    }

    public async Task<Result<CustomerProjection?, Exception>> GetCustomerByFullNameAsync(string firstName, string lastName)
    {
        _logger.Information(nameof(GetCustomerByIdAsync));
        var result = await _context.CustomerProjections.FirstOrDefaultAsync(x => x.FirstName.Equals(firstName) && x.LastName.Equals(lastName));
        if (result == null)
        {
            return new Exception("No customer was found !");
        }
        return result;
    }

    public async Task<Result<CustomerProjection?, Exception>> GetCustomerByIdAsync(Guid id)
    {
        _logger.Information(nameof(GetCustomerByIdAsync));
        var result = await _context.CustomerProjections.FindAsync(id);
        if (result == null)
        {
            return new Exception("No customer was found !");
        }
        return result;
    }
}
