using Application.Common.Mapping;
using Contracts.Customer;
using Contracts.Order;
using Domain.Abstraction;
using Domain.Abstraction.Customers;
using Domain.Abstraction.Orders;
using Domain.Common.Helper;
using MediatR;
using Serilog;

namespace Application.Services.Orders.Commands.AddOrder;

public class AddOrderHandler : IRequestHandler<AddOrderCommand, Result<AddOrderResponse, List<string>>>
{
    private readonly ILogger _logger;
    private readonly IEventStore _eventStore;
    private readonly ICustomerRepositoryReader _repositoryCustomer;
    private readonly ICustomerRepositoryWriter _customerRepositoryWriter;
    private readonly IOrderRepositoryWriter _orderRepositoryWriter;

    public AddOrderHandler(ILogger logger, IEventStore eventStore, ICustomerRepositoryReader repositoryCustomer, ICustomerRepositoryWriter customerRepositoryWriter,
        IOrderRepositoryWriter orderRepositoryWriter)
    {
        _logger = logger;
        _eventStore = eventStore;
        _repositoryCustomer = repositoryCustomer;
        _orderRepositoryWriter = orderRepositoryWriter;
        _customerRepositoryWriter = customerRepositoryWriter;
    }

    public async Task<Result<AddOrderResponse, List<string>>> Handle(AddOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(AddOrderHandler));

        var resustCustomer = await _repositoryCustomer.GetCustomerByFullNameAsync(request.FirstName, request.LastName);
        Result<Guid, Exception> resultId;
        OrderCreated @event;
        if (resustCustomer.IsFailure || resustCustomer.Value is null)
        {
            var @eventCustomer = new CustomerCreated(Ulid.NewUlid().ToGuid(), request.FirstName, request.LastName, request.Mail, request.Address, request.Phone, null);
            await _eventStore.SaveAsync(@eventCustomer, cancellationToken);
            DateOnly dateOrder = DateOnly.FromDateTime(DateTime.Now);
            @event = new OrderCreated(Ulid.NewUlid().ToGuid(), dateOrder, dateOrder.AddDays(30), 0, 0, @eventCustomer.Id, request.OrderMovies);
            resultId = await _orderRepositoryWriter.AddAsync(@event.ToDao(@eventCustomer.ToDao()));
        }
        else
        {
            DateOnly dateOrder = DateOnly.FromDateTime(DateTime.Now);
            @event = new OrderCreated(Ulid.NewUlid().ToGuid(), dateOrder, dateOrder.AddDays(30), 0, 0, resustCustomer.Value.Id, request.OrderMovies);
            resultId = await _orderRepositoryWriter.AddAsync(@event.ToDao());
        }

        if (resultId.IsFailure)
        {
            return new List<string> { resultId.Error.Message };
        }

        var result = await _eventStore.SaveAsync(@event, cancellationToken);
        return result.IsSuccess ? new AddOrderResponse(resultId.Value) : new List<string> { result.Error.Message };
    }
}
