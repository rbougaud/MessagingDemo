namespace Domain.Entities;

public class MovieCommand
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
    public Guid MovieId { get; set; }
    public Movie Movie { get; set; }
    public int Quantity { get; set; }
}
