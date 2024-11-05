namespace Domain.Entities;

public class AuditEntry
{
    public Guid Id { get; set; }
    public string MetaData { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool Succeeded { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}
