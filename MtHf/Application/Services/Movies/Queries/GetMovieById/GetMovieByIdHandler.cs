using Application.Common.Mapping;
using Application.Services.Customers.Queries.GetCustomerById;
using Domain.Abstraction.Movies;
using Domain.Common.Helper;
using MediatR;
using Serilog;

namespace Application.Services.Movies.Queries.GetMovieById;

public class GetMovieByIdHandler : IRequestHandler<GetMovieByIdQuery, Result<GetMovieByIdResponse, string>>
{
    private readonly ILogger _logger;
    private readonly IMovieRepositoryReader _movieRepository;

    public GetMovieByIdHandler(ILogger logger, IMovieRepositoryReader movieRepository)
    {
        _logger = logger;
        _movieRepository = movieRepository;
    }

    public async Task<Result<GetMovieByIdResponse, string>> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(GetCustomerByIdQueryHandler));
        var result = await _movieRepository.GetByIdAsync(request.MovieId);
        return result.IsSuccess ? new GetMovieByIdResponse(result.Value?.ToDto()!) : result.Error.Message;
    }
}
