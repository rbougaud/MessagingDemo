using Domain.Abstraction;

namespace Domain.Entities.Projections;

public class LoyaltyAccountProjection : ISoftDeletable
{
    public Guid Id { get; set; }
    public int Points { get; set; }
    public Guid CustomerId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
    public CustomerProjection Customer { get; set; }
}
