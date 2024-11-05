namespace Domain.Abstraction.Movies;

public interface IMovieDto
{
    Guid Id { get; init; }
    string Title { get; init; }
    string Author { get; init; }
    DateOnly ReleaseDate { get; init; }
    double PurchasePrice { get; init; }
    double SalePrice { get; init; }
}
