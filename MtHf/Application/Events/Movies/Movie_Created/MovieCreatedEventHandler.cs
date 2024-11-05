using Domain.Entities.Projections;
using Infrastructure.Persistence.Context;
using MediatR;
using Serilog;

namespace Application.Events.Movies.Movie_Created;

public class MovieCreatedEventHandler(WriterContext context, ILogger logger) : INotificationHandler<MovieCreatedEvent>
{
    private readonly ILogger _logger = logger;
    private readonly WriterContext _context = context;

    public async Task Handle(MovieCreatedEvent @event, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(MovieCreatedEventHandler));
        try
        {
            var existingProjection = await _context.MovieProjections.FindAsync(@event.MovieCreated.Id);
            if (existingProjection is null)
            {
                var projection = new MovieProjection
                {
                    Id = @event.MovieCreated.Id,
                    Title = @event.MovieCreated.Title,
                    Author = @event.MovieCreated.Author,
                    PurchasePrice = @event.MovieCreated.PurchasePrice,
                    ReleaseDate = @event.MovieCreated.ReleaseDate,
                    SalePrice = @event.MovieCreated.SalePrice
                };
                _context.MovieProjections.Add(projection);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
        }
    }
}
