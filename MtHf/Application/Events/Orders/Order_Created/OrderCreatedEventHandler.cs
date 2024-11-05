using Domain.Common.Enums;
using Domain.Entities.Projections;
using Infrastructure.Persistence.Context;
using MediatR;
using Serilog;

namespace Application.Events.Orders.Order_Created;

public class OrderCreatedEventHandler(ILogger logger, WriterContext context) : INotificationHandler<OrderCreatedEvent>
{
    private readonly ILogger _logger = logger;
    private readonly WriterContext _context = context;

    public async Task Handle(OrderCreatedEvent @event, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(OrderCreatedEventHandler));
        try
        {
            var existingCustomerProjection = await _context.CustomerProjections.FindAsync(@event.OrderCreated.CustomerId);
            if (existingCustomerProjection is null)
            {
                _logger.Information("Customer with id {CustomerId} is null", @event.OrderCreated.CustomerId);
                Thread.Sleep(1000);
                existingCustomerProjection = await _context.CustomerProjections.FindAsync(@event.OrderCreated.CustomerId);
                if (existingCustomerProjection is null)
                {
                    _logger.Error("projection customer with id {CustomerId} is null", @event.OrderCreated.CustomerId);
                    return;
                }
            }

            var existingProjection = await _context.OrderProjections.FindAsync(@event.OrderCreated.Id);
            if (existingProjection == null)
            {
                ICollection<MovieCommandProjection> collection = [];
                foreach (var item in @event.OrderCreated.OrderMovies)
                {
                    collection.Add(new MovieCommandProjection { OrderId = @event.OrderCreated.Id, MovieId = item.Key, Quantity = item.Value });
                }

                var projection = new OrderProjection
                {
                    Id = @event.OrderCreated.Id,
                    DateOrder = @event.OrderCreated.DateOrder,
                    DueDate = @event.OrderCreated.DueDate,
                    PaymentMode = (short)PaymentMode.None,
                    DeliveryMode = @event.OrderCreated.DeliveryMode,
                    State = @event.OrderCreated.State,
                    CustomerId = @event.OrderCreated.CustomerId,
                    MovieCommands = collection
                };
                _context.OrderProjections.Add(projection);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
        }
    }
}
