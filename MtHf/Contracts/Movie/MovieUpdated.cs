namespace Contracts.Movie;

public record MovieUpdated(Guid Id, string Title, string Author, DateOnly ReleaseDate, double PurchasePrice, double Saleprice);
