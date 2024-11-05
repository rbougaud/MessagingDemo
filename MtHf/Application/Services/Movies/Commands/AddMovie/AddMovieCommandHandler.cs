using Contracts.Movie;
using Domain.Abstraction;
using Domain.Abstraction.Movies;
using Domain.Common.Constantes;
using Domain.Common.Helper;
using MediatR;
using Serilog;
using Application.Common.Mapping;
using Hangfire;

namespace Application.Services.Movies.Commands.AddMovie;

public class AddMovieCommandHandler : IRequestHandler<AddMovieCommand, Result<AddMovieResponse, List<string>>>
{
    private readonly ILogger _logger;
    private readonly IEventStore _eventStore;
    private readonly IMovieRepositoryWriter _repositoryMovie;
    private readonly IProcessOutboxMessagesJob _processor;

    public AddMovieCommandHandler(ILogger logger, IEventStore eventStore, IMovieRepositoryWriter movieRepository, IProcessOutboxMessagesJob processor)
    {
        _logger = logger;
        _eventStore = eventStore;
        _repositoryMovie = movieRepository;
        _processor = processor;
    }

    public async Task<Result<AddMovieResponse, List<string>>> Handle(AddMovieCommand request, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(AddMovieCommandHandler));
        var resultExist = _repositoryMovie.CheckIfExist(request.Title, request.Author);
        if (resultExist.IsFailure)
        {
            return new List<string> { resultExist.Error.Message };
        }

        double salingPrice = Math.Round(request.PurchasePrice * (1 + Const.MOVIE_MARGING) * (1 + Const.TVA_RATE), 2);
        var @event = new MovieCreated(Ulid.NewUlid().ToGuid(), request.Title, request.Author, request.ReleaseDate, request.PurchasePrice, salingPrice);
        var result = await _eventStore.SaveAsync(@event, cancellationToken);
        BackgroundJob.Enqueue(() => _processor.ProcessAsync());
        if (result.IsFailure)
        {
            return new List<string> { result.Error.Message };
        }
        var resultId = await _repositoryMovie.AddAsync(@event.ToDao());
        return resultId.IsSuccess ? new AddMovieResponse(resultId.Value.ToString()) : new List<string> { resultId.Error.Message };
    }
}
