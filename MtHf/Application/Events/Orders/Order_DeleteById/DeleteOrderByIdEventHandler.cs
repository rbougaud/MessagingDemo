using Infrastructure.Persistence.Context;
using MediatR;
using Serilog;

namespace Application.Events.Orders.Order_DeleteById;

public class DeleteOrderByIdEventHandler(ILogger logger, WriterContext context) : INotificationHandler<DeleteOrderByIdEvent>
{
    private readonly ILogger _logger = logger;
    private readonly WriterContext _context = context;

    public async Task Handle(DeleteOrderByIdEvent @event, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(DeleteOrderByIdEventHandler));
        try
        {
            var existingProjection = await _context.OrderProjections.FindAsync(@event.OrderId);
            if (existingProjection is not null)
            {
                _context.OrderProjections.Remove(existingProjection);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
        }
    }
}
