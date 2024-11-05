namespace Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Mail { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Iban { get; set; }

    public LoyaltyAccount? LoyaltyAccount { get; set; }
    public CreditCard? CreditCard { get; set; }
    public virtual ICollection<Order>? Orders { get; set; }
}
