using Contracts.Order;
using Domain.Abstraction;
using Domain.Abstraction.Orders;
using Domain.Common.Helper;
using MediatR;
using Serilog;

namespace Application.Services.Orders.Commands.UpdateDeliveryOrder;

public class UpdateDeliveryOrderHandler : IRequestHandler<UpdateDeliveryOrderCommand, Result<UpdateDeliveryOrderResponse, List<string>>>
{
    private readonly ILogger _logger;
    private readonly IEventStore _eventStore;
    private readonly IOrderRepositoryWriter _orderRepositoryWriter;

    public UpdateDeliveryOrderHandler(ILogger logger, IEventStore eventStore, IOrderRepositoryWriter orderRepositoryWriter)
    {
        _logger = logger;
        _eventStore = eventStore;
        _orderRepositoryWriter = orderRepositoryWriter;
    }

    public async Task<Result<UpdateDeliveryOrderResponse, List<string>>> Handle(UpdateDeliveryOrderCommand cmd, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(UpdateDeliveryOrderHandler));

        var result = await _orderRepositoryWriter.UpdateDeliveryAsync(cmd.OrderId, cmd.State, cmd.DeliveryMode, cmd.DeliveryDate);
        if (result.IsSuccess)
        {
            var @event = new OrderDeliveryOptionsUpdated(cmd.OrderId, cmd.State, cmd.DeliveryMode, cmd.DeliveryDate);
            result = await _eventStore.SaveAsync(@event, cancellationToken);
        }
        return result.IsSuccess ? new UpdateDeliveryOrderResponse(result.Value) : new List<string> { result.Error.Message };
    }
}
