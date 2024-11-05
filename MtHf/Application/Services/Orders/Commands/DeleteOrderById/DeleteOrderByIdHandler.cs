using Contracts.Order;
using Domain.Abstraction;
using Domain.Abstraction.Orders;
using Domain.Common.Helper;
using MediatR;
using Serilog;

namespace Application.Services.Orders.Commands.DeleteOrderById;

public class DeleteOrderByIdHandler : IRequestHandler<DeleteOrderByIdCommand, Result<DeleteOrderByIdResponse, List<string>>>
{
    private readonly ILogger _logger;
    private readonly IEventStore _eventStore;
    private readonly IOrderRepositoryWriter _orderRepositoryWriter;

    public DeleteOrderByIdHandler(ILogger logger, IEventStore eventStore, IOrderRepositoryWriter orderRepositoryWriter)
    {
        _logger = logger;
        _eventStore = eventStore;
        _orderRepositoryWriter = orderRepositoryWriter;
    }

    public async Task<Result<DeleteOrderByIdResponse, List<string>>> Handle(DeleteOrderByIdCommand request, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(DeleteOrderByIdHandler));
        var result = await _orderRepositoryWriter.DeleteByIdAsync(request.OrderId);
        if (result.IsSuccess && result.Value)
        {
            var @event = new OrderDeleted(request.OrderId);
            result = await _eventStore.SaveAsync(@event, cancellationToken);
        }
        return result.IsSuccess ? new DeleteOrderByIdResponse(result.Value) : new List<string> { result.Error.Message };
    }
}
