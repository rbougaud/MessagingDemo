using Application.Common.Mapping;
using Contracts.Movie;
using Domain.Abstraction;
using Domain.Abstraction.Movies;
using Domain.Common.Constantes;
using Domain.Common.Helper;
using MediatR;
using Serilog;

namespace Application.Services.Movies.Commands.UpdateMovie;

public class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, Result<UpdateMovieResponse, List<string>>>
{
    private readonly ILogger _logger;
    private readonly IEventStore _eventStore;
    private readonly IMovieRepositoryWriter _repositoryMovie;

    public UpdateMovieCommandHandler(ILogger logger, IEventStore eventStore, IMovieRepositoryWriter movieRepository)
    {
        _logger = logger;
        _eventStore = eventStore;
        _repositoryMovie = movieRepository;
    }

    public async Task<Result<UpdateMovieResponse, List<string>>> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(UpdateMovieCommandHandler));
        double salingPrice = request.SalePrice;
        if (request.SalePrice < request.PurchasePrice)
        {
            salingPrice = request.PurchasePrice * (1 + Const.MOVIE_MARGING);
        }
        double priceTtc = Math.Round(salingPrice * (1 + Const.TVA_RATE), 2);
        var @event = new MovieUpdated(request.Id, request.Title, request.Author, request.ReleaseDate, request.PurchasePrice, priceTtc);
        var result = await _eventStore.SaveAsync(@event, cancellationToken);
        if (result.IsSuccess)
        {
            result = await _repositoryMovie.UpdateAsync(@event.ToDao());
        }
        return result.IsSuccess ? new UpdateMovieResponse(result.Value) : new List<string> { result.Error.Message };
    }
}
