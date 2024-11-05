using Infrastructure.Persistence.Context;
using MediatR;
using Serilog;

namespace Application.Events.Movies.Movie_Updated;

public class MovieUpdatedEventHandler(WriterContext context, ILogger logger) : INotificationHandler<MovieUpdatedEvent>
{
    private readonly ILogger _logger = logger;
    private readonly WriterContext _context = context;

    public async Task Handle(MovieUpdatedEvent @event, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(MovieUpdatedEventHandler));
        try
        {
            var existingProjection = await _context.MovieProjections.FindAsync(@event.MovieUpdated.Id);
            if (existingProjection is not null)
            {
                existingProjection.ReleaseDate = @event.MovieUpdated.ReleaseDate;
                existingProjection.PurchasePrice = @event.MovieUpdated.PurchasePrice;
                existingProjection.SalePrice = @event.MovieUpdated.Saleprice;

                _context.MovieProjections.Update(existingProjection);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
        }
    }
}
