using Domain.Abstraction.Movies;

namespace Application.Common.Dto.Movies;

public record MovieDto : IMovieDto
{
    public Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Author { get; init; }
    public DateOnly ReleaseDate { get; init; }
    public double PurchasePrice { get; init; }
    public double SalePrice { get; init; }
}
