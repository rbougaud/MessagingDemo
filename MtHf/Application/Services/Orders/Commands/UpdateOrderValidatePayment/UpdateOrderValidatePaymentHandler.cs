using Contracts.Order;
using Domain.Abstraction;
using Domain.Abstraction.Orders;
using Domain.Common.Enums;
using Domain.Common.Helper;
using MediatR;
using Serilog;

namespace Application.Services.Orders.Commands.UpdateOrderValidatePayment;

public class UpdateOrderValidatePaymentHandler : IRequestHandler<UpdateOrderValidatePaymentCommand, Result<UpdateOrderValidatePaymentResponse, List<string>>>
{
    private readonly ILogger _logger;
    private readonly IEventStore _eventStore;
    private readonly IOrderRepositoryWriter _orderRepositoryWriter;

    public UpdateOrderValidatePaymentHandler(ILogger logger, IEventStore eventStore, IOrderRepositoryWriter orderRepositoryWriter)
    {
        _logger = logger;
        _eventStore = eventStore;
        _orderRepositoryWriter = orderRepositoryWriter;
    }

    public async Task<Result<UpdateOrderValidatePaymentResponse, List<string>>> Handle(UpdateOrderValidatePaymentCommand cmd, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(UpdateOrderValidatePaymentHandler));

        var result = await _orderRepositoryWriter.UpdateStateAsync(cmd.OrderId, StateOrder.PendingPayment, cmd.PaymentMode);
        if (result.IsSuccess)
        {
            var @event = new OrderPaymentValidated(cmd.OrderId, cmd.PaymentMode, (short)StateOrder.PendingPayment);
            result = await _eventStore.SaveAsync(@event, cancellationToken);
        }
        return result.IsSuccess ? new UpdateOrderValidatePaymentResponse(result.Value) : new List<string> { result.Error.Message };
    }
}
