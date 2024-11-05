using Application.Common.Mapping;
using Domain.Abstraction.Orders;
using Domain.Common.Helper;
using MediatR;
using Serilog;

namespace Application.Services.Orders.Queries.GetOrders;

public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, Result<GetOrdersResponse, string>>
{
    private readonly ILogger _logger;
    private readonly IOrderRepositoryReader _orderRepositoryReader;

    public GetOrdersHandler(ILogger logger, IOrderRepositoryReader orderRepositoryReader)
    {
        _logger = logger;
        _orderRepositoryReader = orderRepositoryReader;
    }

    public async Task<Result<GetOrdersResponse, string>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(GetOrdersHandler));
        var result = await _orderRepositoryReader.GetAllAsync();
        return result.IsSuccess ? new GetOrdersResponse(result.Value.Select(x => x.ToDto()).ToList()) : result.Error.Message;
    }
}
