using Domain.Abstraction.Movies;
using Domain.Common.Helper;
using Domain.Entities.Projections;
using Infrastructure.Persistence.Context;
using Serilog;

namespace Infrastructure.Persistence.Repositories;

public class MovieRepositoryReader(ILogger logger, ReaderContext context) : IMovieRepositoryReader
{
    private readonly ILogger _logger = logger;
    private readonly ReaderContext _context = context;

    public Result<IEnumerable<MovieProjection>, Exception> GetAllAsync()
    {
        _logger.Information(nameof(GetAllAsync));
        return _context.MovieProjections;
    }

    public async Task<Result<MovieProjection?, Exception>> GetByIdAsync(Guid id)
    {
        _logger.Information(nameof(GetByIdAsync));
        var result = await _context.MovieProjections.FindAsync(id);
        if (result == null)
        {
            return new Exception("No Movie found");
        }
        return result;
    }
}
