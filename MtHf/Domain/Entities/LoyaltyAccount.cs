using Domain.Abstraction;

namespace Domain.Entities;

public class LoyaltyAccount : ISoftDeletable
{
    public Guid Id { get; set; }
    public int Points { get; set; }
    public Guid CustomerId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
    public Customer Customer { get; set; }
}

