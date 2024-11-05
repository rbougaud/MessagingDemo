using Application.Common.Mapping;
using Domain.Abstraction.Customers;
using Domain.Common.Helper;
using MediatR;
using Serilog;

namespace Application.Services.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Result<GetCustomerByIdResponse, string>>
{
    private readonly ILogger _logger;
    private readonly ICustomerRepositoryReader _customerRepositoryReader;

    public GetCustomerByIdQueryHandler(ILogger logger, ICustomerRepositoryReader repositoryReader)
    {
        _logger = logger;
        _customerRepositoryReader = repositoryReader;
    }

    public async Task<Result<GetCustomerByIdResponse, string>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(GetCustomerByIdQueryHandler));
        var result = await _customerRepositoryReader.GetCustomerByIdAsync(request.Id);
        return result.IsSuccess ? new GetCustomerByIdResponse(result.Value?.ToDto()!) : result.Error.Message;
    }
}
