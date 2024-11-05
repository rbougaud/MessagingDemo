using Application.Common.Dto.Movies;
using Application.Common.Mapping;
using Domain.Abstraction.Movies;
using Domain.Common.Helper;
using MediatR;
using Serilog;

namespace Application.Services.Movies.Queries.GetMovies;

public class GetMoviesHandler : IRequestHandler<GetMoviesQuery, Result<GetMoviesResponse, string>>
{
    private readonly ILogger _logger;
    private readonly IMovieRepositoryReader _movieRepository;

    public GetMoviesHandler(ILogger logger, IMovieRepositoryReader movieRepository)
    {
        _logger = logger;
        _movieRepository = movieRepository;
    }

    public async Task<Result<GetMoviesResponse, string>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(GetMoviesHandler));
        var result = _movieRepository.GetAllAsync();
        return result.IsSuccess ? new GetMoviesResponse(result.Value.Select(x => x.ToDto()).OfType<MovieDto>().ToList()) : result.Error.Message;
    }
}
