using Domain.Abstraction.Orders;
using Domain.Common.Enums;
using Domain.Common.Helper;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Serilog;

namespace Infrastructure.Persistence.Repositories;

public class OrderRepositoryWriter(ILogger logger, WriterContext context) : IOrderRepositoryWriter
{
    private readonly ILogger _logger = logger;
    private readonly WriterContext _context = context;

    public async Task<Result<Guid, Exception>> AddAsync(Order order)
    {
        _logger.Information(nameof(AddAsync));
        _context.Orders.Add(order);
        var result = await _context.SaveChangesAsync();
        return result > 0 ? order.Id : Guid.Empty;
    }

    public async Task<Result<bool, Exception>> DeleteByIdAsync(Guid id)
    {
        _logger.Information(nameof(DeleteByIdAsync));
        var existingOrder = await _context.Orders.FindAsync(id);
        if (existingOrder is null)
        {
            return new Exception("This order does not exist !");
        }
        _context.Orders.Remove(existingOrder);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<Result<bool, Exception>> UpdateDeliveryAsync(Guid id, short state, short deliveryMode, DateOnly deliveryDate)
    {
        _logger.Information(nameof(UpdateDeliveryAsync));
        try
        {
            var existingOrder = await _context.Orders.FindAsync(id);
            if (existingOrder is null)
            {
                return new Exception("This customer does not exist !");
            }

            existingOrder.State = state;
            existingOrder.DeliveryMode = deliveryMode;
            existingOrder.DeliveryDate = deliveryDate;

            _context.Orders.Update(existingOrder);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ex;
        }
    }

    public async Task<Result<bool, Exception>> UpdateStateAsync(Guid orderId, StateOrder state, short paymentMode)
    {
        _logger.Information(nameof(UpdateStateAsync));
        try
        {
            var existingOrder = await _context.Orders.FindAsync(orderId);
            if (existingOrder is null)
            {
                return new Exception("This customer does not exist !");
            }

            existingOrder.State = (short)state;
            existingOrder.PaymentMode = paymentMode;

            _context.Orders.Update(existingOrder);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ex;
        }
    }

    public async Task<Result<bool, Exception>> UpdateAsync(Order order)
    {
        _logger.Information(nameof(UpdateAsync));
        try
        {
            var existingOrder = await _context.Orders.FindAsync(order.Id);
            if (existingOrder is null)
            {
                return new Exception("This customer does not exist !");
            }

            existingOrder.DeliveryMode = order.DeliveryMode;
            existingOrder.DeliveryDate = order.DeliveryDate;
            existingOrder.State = order.State;
            existingOrder.MovieCommands = order.MovieCommands;

            _context.Orders.Update(existingOrder);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ex;
        }
    }
}
