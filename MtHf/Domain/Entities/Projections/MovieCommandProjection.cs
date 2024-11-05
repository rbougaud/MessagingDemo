namespace Domain.Entities.Projections;

public class MovieCommandProjection
{
    public Guid OrderId { get; set; }
    public OrderProjection Order { get; set; }
    public Guid MovieId { get; set; }
    public MovieProjection Movie { get; set; }
    public int Quantity { get; set; }
}
