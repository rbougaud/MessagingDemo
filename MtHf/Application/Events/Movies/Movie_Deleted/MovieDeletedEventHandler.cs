using Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.Events.Movies.Movie_Deleted;

public class MovieDeletedEventHandler(WriterContext context, ILogger logger) : INotificationHandler<MovieDeletedEvent>
{
    private readonly ILogger _logger = logger;
    private readonly WriterContext _context = context;

    public async Task Handle(MovieDeletedEvent @event, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(MovieDeletedEventHandler));
        try
        {
            var existingProjections = await _context.MovieProjections.Where(x => x.Title.Equals(@event.MovieDeleted.Title) 
                                                                              && x.Author.Equals(@event.MovieDeleted.Author)).ToListAsync();
            if (existingProjections is not null && existingProjections.Count != 0)
            {
                _context.MovieProjections.RemoveRange(existingProjections);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
        }
    }
}
