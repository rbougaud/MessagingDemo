namespace Domain.Entities.Projections;

public class CreditCardProjection
{
    public Guid Id { get; set; }
    public required string HolderName { get; set; }
    public required string CardType { get; set; }
    public required string CardNumber { get; set; }
    public DateOnly ExpiryDate { get; set; }
    public required string MvcCode { get; set; }
    public Guid CustomerId { get; set; }
    public CustomerProjection Customer { get; set; }
}
