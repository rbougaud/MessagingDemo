using Contracts.Movie;
using Domain.Abstraction;
using Domain.Abstraction.Movies;
using Domain.Common.Helper;
using MediatR;
using Serilog;

namespace Application.Services.Movies.Commands.DeleteMovieByTitle;

public class DeleteMovieByTitleCommandHandler : IRequestHandler<DeleteMovieByTitleCommand, Result<DeleteMovieByTitleCommandResponse, List<string>>>
{
    private readonly ILogger _logger;
    private readonly IEventStore _eventStore;
    private readonly IMovieRepositoryWriter _repositoryMovie;

    public DeleteMovieByTitleCommandHandler(ILogger logger, IEventStore eventStore, IMovieRepositoryWriter movieRepository)
    {
        _logger = logger;
        _eventStore = eventStore;
        _repositoryMovie = movieRepository;
    }

    public async Task<Result<DeleteMovieByTitleCommandResponse, List<string>>> Handle(DeleteMovieByTitleCommand request, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(DeleteMovieByTitleCommandHandler));
        var result = await _repositoryMovie.DeleteByTitleAsync(request.Title, request.Author);
        if (result.IsSuccess && result.Value)
        {
            var @event = new MovieDeleted(request.Id, request.Title, request.Author);
            result = await _eventStore.SaveAsync(@event, cancellationToken);
            return result.IsFailure ? new List<string> { result.Error.Message } : new DeleteMovieByTitleCommandResponse(true);
        }
        return result.IsSuccess ? new DeleteMovieByTitleCommandResponse(result.Value) : new List<string> { result.Error.Message };
    }
}
