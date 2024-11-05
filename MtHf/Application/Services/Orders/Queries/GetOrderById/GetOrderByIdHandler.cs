using Application.Common.Mapping;
using Domain.Abstraction.Orders;
using Domain.Common.Helper;
using MediatR;
using Serilog;

namespace Application.Services.Orders.Queries.GetOrderById;

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, Result<GetOrderByIdResponse, string>>
{
    private readonly ILogger _logger;
    private readonly IOrderRepositoryReader _orderRepositoryReader;

    public GetOrderByIdHandler(ILogger logger, IOrderRepositoryReader orderRepositoryReader)
    {
        _logger = logger;
        _orderRepositoryReader = orderRepositoryReader;
    }

    public async Task<Result<GetOrderByIdResponse, string>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(GetOrderByIdHandler));
        var result = await _orderRepositoryReader.GetByIdAsync(request.OrderId);
        return result.IsSuccess ? new GetOrderByIdResponse(result.Value?.ToDto()!) : result.Error.Message;
    }
}
