using Application.Common.Dto.Customers;
using Application.Common.Mapping;
using Application.Services.Movies.Queries.GetMovies;
using Domain.Abstraction.Customers;
using Domain.Common.Helper;
using MediatR;
using Serilog;

namespace Application.Services.Customers.Queries.GetCustomers;

public class GetCustomersHandler : IRequestHandler<GetCustomersQuery, Result<GetCustomersResponse, string>>
{
    private readonly ILogger _logger;
    private readonly ICustomerRepositoryReader _customerRepositoryReader;

    public GetCustomersHandler(ILogger logger, ICustomerRepositoryReader repositoryReader)
    {
        _logger = logger;
        _customerRepositoryReader = repositoryReader;
    }

    public async Task<Result<GetCustomersResponse, string>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(GetMoviesHandler));
        var result = await _customerRepositoryReader.GetAllAsync();
        return result.IsSuccess ? new GetCustomersResponse(result.Value.Select(x => x.ToDto()).OfType<CustomerDto>().ToList()) : result.Error.Message;
    }
}
