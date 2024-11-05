using Domain.Abstraction.Orders;
using Domain.Common.Enums;
using Domain.Common.Helper;
using Domain.Entities.Projections;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Persistence.Repositories;

public class OrderRepositoryReader(ILogger logger, ReaderContext context) : IOrderRepositoryReader
{
    private readonly ILogger _logger = logger;
    private readonly ReaderContext _context = context;

    public async Task<Result<IEnumerable<OrderProjection>, Exception>> GetAllAsync()
    {
        _logger.Information(nameof(GetAllAsync));
        return await _context.OrderProjections.Include(x => x.Customer).Include(x => x.MovieCommands).ThenInclude(y => y.Movie).ToListAsync();
    }

    public async Task<Result<OrderProjection, Exception>> GetByIdAsync(Guid id)
    {
        _logger.Information(nameof(GetByIdAsync));
        var result = await _context.OrderProjections.Include(x => x.Customer).ThenInclude(y => y.CreditCard)
                                                    .Include(x => x.Customer).ThenInclude(y => y.LoyaltyAccount)
                                                    .Include(x => x.MovieCommands).ThenInclude(y => y.Movie).SingleOrDefaultAsync(x => x.Id == id);
        if (result == null)
        {
            return new Exception("No Movie found");
        }
        return result;
    }

    public async Task<List<OrderProjection>> GetPendingOrders()
    {
        return await _context.OrderProjections.Include(x => x.Customer).ThenInclude(y => y.CreditCard)
                                              .Include(x => x.Customer).ThenInclude(y => y.LoyaltyAccount)
                                              .Include(x => x.MovieCommands).ThenInclude(mc => mc.Movie)
                                              .Where(x => x.State == (short)StateOrder.PendingPayment)
                                              .ToListAsync();
    }

    public async Task<List<OrderProjection>> GetReqPendingOrders()
    {
        _logger.Information(nameof(GetReqPendingOrders));
        const string sql = @"
        SELECT
            o.Id as OrderId, o.DateOrder, o.DueDate, o.DatePayment, o.DeliveryMode, o.DeliveryDate, o.State, o.CustomerId,
            c.Id, c.FirstName, c.LastName, c.Mail, c.Address, c.Phone, c.Iban,
            cc.Id AS CreditCardId, cc.HolderName, cc.CardType, cc.CardNumber, cc.ExpiryDate, cc.MvcCode, cc.CustomerId AS CCCustomerId,
            la.Id AS LoyaltyAccountId, la.Points, la.CustomerId AS LACustomerId
        FROM OrderProjection o
        LEFT JOIN CustomerProjection c ON o.CustomerId = c.Id
        LEFT JOIN CreditCardProjection cc ON c.Id = cc.CustomerId
        LEFT JOIN LoyaltyAccountProjection la ON c.Id = la.CustomerId
        WHERE o.State = 2";

        try
        {
            var result = await _context.OrderProjections.FromSqlRaw(sql).Select(o => new OrderProjection
            {
                Id = o.Id,
                DateOrder = o.DateOrder,
                DatePayment = o.DatePayment,
                DeliveryDate = o.DeliveryDate,
                DeliveryMode = o.DeliveryMode,
                DueDate = o.DueDate,
                MovieCommands = o.MovieCommands,
                State = o.State,
                Customer = new CustomerProjection
                {
                    Id = o.Customer.Id,
                    FirstName = o.Customer.FirstName,
                    LastName = o.Customer.LastName,
                    Mail = o.Customer.Mail,
                    Address = o.Customer.Address,
                    Phone = o.Customer.Phone,
                    Iban = o.Customer.Iban,
                    CreditCard = o.Customer.CreditCard,
                    LoyaltyAccount = o.Customer.LoyaltyAccount != null ? new LoyaltyAccountProjection
                    {
                        Id = o.Customer.LoyaltyAccount.Id,
                        Points = o.Customer.LoyaltyAccount.Points,
                        CustomerId = o.Customer.LoyaltyAccount.CustomerId
                    } : null
                },
                CustomerId = o.CustomerId,
            }).ToListAsync();
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return [];
        }
    }
}
