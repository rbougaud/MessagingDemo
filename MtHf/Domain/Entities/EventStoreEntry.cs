namespace Domain.Entities;

public class EventStoreEntry
{
    public Guid Id { get; set; }
    public required string AggregateId { get; set; }
    public required string EventType { get; set; }
    public required string Data { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

