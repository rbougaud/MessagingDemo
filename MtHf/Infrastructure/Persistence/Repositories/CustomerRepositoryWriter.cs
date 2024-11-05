using Domain.Abstraction.Customers;
using Domain.Common.Helper;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Persistence.Repositories;

public class CustomerRepositoryWriter(ILogger logger, WriterContext context) : ICustomerRepositoryWriter
{
    private readonly ILogger _logger = logger;
    private readonly WriterContext _context = context;

    public async Task<Result<Guid, Exception>> AddAsync(Customer customer)
    {
        _logger.Information(nameof(AddAsync));
        await _context.Customers.AddAsync(customer);
        var result = await _context.SaveChangesAsync();
        return result > 0 ? customer.Id : Guid.Empty;
    }

    public Result<bool, Exception> CheckCustomerExist(string firstName, string lastName)
    {
        _logger.Information(nameof(CheckCustomerExist));
        bool isExistingCustomer = _context.Customers.Any(x => x.LastName.Equals(lastName) && x.FirstName.Equals(firstName));
        if (isExistingCustomer) { return new Exception("This customer already exist"); }
        return isExistingCustomer;
    }

    public async Task<Result<bool, Exception>> DeleteAsync(string firstName, string lastName)
    {
        _logger.Information(nameof(DeleteAsync));
        var existingCustomer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.FirstName.Equals(firstName) && x.LastName.Equals(lastName));
        if (existingCustomer is null)
        {
            return new Exception("This customer does not exist !");
        }
        _context.Customers.Remove(existingCustomer);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<Result<bool, Exception>> UpdateAsync(Customer customer)
    {
        _logger.Information(nameof(UpdateAsync));
        try
        {
            var existingCustomer = await _context.Customers.AsNoTracking()
                                                           .FirstOrDefaultAsync(x => x.Id == customer.Id 
                                                                             && x.FirstName.Equals(customer.FirstName) 
                                                                             && x.LastName.Equals(customer.LastName));
            if (existingCustomer is null)
            {
                return new Exception("This customer does not exist !");
            }
            existingCustomer.Mail = customer.Mail;
            existingCustomer.Address = customer.Address;
            existingCustomer.Phone = customer.Phone;
            existingCustomer.Iban = customer.Iban;

            _context.Customers.Update(existingCustomer);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ex;
        }
    }
}
