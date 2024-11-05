namespace Domain.Entities.Projections;

public class CustomerProjection
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Mail { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Iban { get; set; }

    public LoyaltyAccountProjection? LoyaltyAccount { get; set; }
    public CreditCardProjection? CreditCard { get; set; }
    public ICollection<OrderProjection>? Orders { get; set; }
}
