namespace Domain.Entities.Projections;

public class MovieProjection
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public double PurchasePrice { get; set; }
    public double SalePrice { get; set; }

    public ICollection<MovieCommandProjection> MovieCommands { get; set; }
}
