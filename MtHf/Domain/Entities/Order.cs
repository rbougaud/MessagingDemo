namespace Domain.Entities;

public class Order
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
    public virtual Customer Customer { get; set; }
    public virtual ICollection<MovieCommand> MovieCommands { get; set; }
}
