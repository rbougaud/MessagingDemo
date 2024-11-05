using Domain.Abstraction.Movies;
using Domain.Common.Helper;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Persistence.Repositories;

public class MovieRepositoryWriter(ILogger logger, WriterContext context) : IMovieRepositoryWriter
{
    private readonly ILogger _logger = logger;
    private readonly WriterContext _context = context;


    public async Task<Result<Guid, Exception>> AddAsync(Movie movie)
    {
        _logger.Information(nameof(AddAsync));
        _context.Movies.Add(movie);
        var result = await _context.SaveChangesAsync();
        return result > 0 ? movie.Id : Guid.Empty;
    }

    public Result<bool, Exception> CheckIfExist(string title, string author) //TODO RBO Voir to reader
    {
        _logger.Information(nameof(CheckIfExist));
        bool isExistingMovie = _context.Movies.Any(x => x.Title.Equals(title) && x.Author.Equals(author));
        if (isExistingMovie) { return new Exception("This movie already exist"); }
        return isExistingMovie;
    }

    public async Task<Result<bool, Exception>> DeleteByTitleAsync(string title, string author)
    {
        _logger.Information(nameof(DeleteByTitleAsync));
        var existingMovie = await _context.Movies.AsNoTracking().FirstOrDefaultAsync(x => x.Title.Equals(title) && x.Author.Equals(author));
        if (existingMovie is null)
        {
            return new Exception("This movie does not exist !");
        }
        _context.Movies.Remove(existingMovie);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<Result<bool, Exception>> UpdateAsync(Movie movie)
    {
        _logger.Information(nameof(UpdateAsync));
        try
        {
            var existingMovie = await _context.Movies.AsNoTracking().FirstOrDefaultAsync(x => x.Title.Equals(movie.Title) && x.Author.Equals(movie.Author));
            if (existingMovie is null)
            {
                return new Exception("This customer does not exist !");
            }
            existingMovie.ReleaseDate = movie.ReleaseDate;
            existingMovie.PurchasePrice = movie.PurchasePrice;
            existingMovie.SalePrice = movie.SalePrice;

            _context.Movies.Update(existingMovie);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ex;
        }
    }
}
