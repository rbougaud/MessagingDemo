using Domain.Abstraction.CreditCards;
using Domain.Common.Helper;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Persistence.Repositories;

public class CreditCardRepositoryWriter(ILogger logger, WriterContext context) : ICreditCardRepositoryWriter
{
    private readonly ILogger _logger = logger;
    private readonly WriterContext _context = context;

    public async Task<Result<Guid, Exception>> AddAsync(CreditCard creditCard)
    {
        _logger.Information(nameof(AddAsync));
        _context.CreditCards.Add(creditCard);
        var result = await _context.SaveChangesAsync();
        return result > 0 ? creditCard.Id : Guid.Empty;
    }

    public async Task<Result<bool, Exception>> DeleteByCustomerIdAsync(Guid customerId)
    {
        _logger.Information(nameof(DeleteByCustomerIdAsync));
        var existingCreditCard = await _context.CreditCards.AsNoTracking().FirstOrDefaultAsync(x => x.CustomerId == customerId);
        if (existingCreditCard is null)
        {
            return new Exception("This credit card does not exist !");
        }
        _context.CreditCards.Remove(existingCreditCard);
        return await _context.SaveChangesAsync() > 0;
    }
}
