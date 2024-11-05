namespace Domain.Entities.Projections;

public class OrderProjection
{
    public Guid Id { get; set; }
    public DateOnly DateOrder { get; set; }
    public DateOnly DueDate { get; set; }
    public short PaymentMode { get; set; }
    public DateOnly DatePayment { get; set; }
    public short DeliveryMode { get; set; }
    public DateOnly DeliveryDate { get; set; }
    public short State { get; set; }
    public Guid CustomerId { get; set; }
    public CustomerProjection Customer { get; set; }
    public ICollection<MovieCommandProjection> MovieCommands { get; set; } = [];
}
