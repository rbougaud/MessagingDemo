using Domain.Common.Helper;
using Domain.Entities.Projections;

namespace Domain.Abstraction.Movies;

public interface IMovieRepositoryReader
{
    Task<Result<MovieProjection?, Exception>> GetByIdAsync(Guid id);

    Result<IEnumerable<MovieProjection>, Exception> GetAllAsync();
}
