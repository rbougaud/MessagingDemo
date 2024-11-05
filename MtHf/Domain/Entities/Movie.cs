namespace Domain.Entities;

public class Movie
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public double PurchasePrice { get; set; }
    public double SalePrice { get; set; }

    public virtual ICollection<MovieCommand> MovieCommands { get; set; }
}
