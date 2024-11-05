using Domain.Abstraction.Movies;

namespace Application.Common.Dto.Movies;

public class MovieDtoNotFound : IMovieDto
{
    public Guid Id { get; init; } = Guid.Empty;
    public string Title { get; init; } = string.Empty;
    public string Author { get; init; } = string.Empty;
    public DateOnly ReleaseDate { get; init; }
    public double PurchasePrice { get; init; }
    public double SalePrice { get; init; }
}
