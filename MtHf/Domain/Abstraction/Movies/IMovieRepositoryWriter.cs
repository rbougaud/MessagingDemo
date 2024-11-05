using Domain.Common.Helper;
using Domain.Entities;

namespace Domain.Abstraction.Movies;

public interface IMovieRepositoryWriter
{
    Task<Result<Guid, Exception>> AddAsync(Movie customer);
    Result<bool, Exception> CheckIfExist(string title, string author);
    Task<Result<bool, Exception>> DeleteByTitleAsync(string title, string author);
    Task<Result<bool, Exception>> UpdateAsync(Movie customer);
}
