namespace Domain.Entities;

public class OutboxMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Payload { get; set; }
    public DateTime OccurredOn { get; set; }
    public bool Processed { get; set; }
}

