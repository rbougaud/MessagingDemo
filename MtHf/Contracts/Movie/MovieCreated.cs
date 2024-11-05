namespace Contracts.Movie;

public record MovieCreated(Guid Id, string Title, string Author, DateOnly ReleaseDate, double PurchasePrice, double SalePrice);
