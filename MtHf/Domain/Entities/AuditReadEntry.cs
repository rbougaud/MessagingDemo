namespace Domain.Entities;

public class AuditReadEntry
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string QueryText { get; set; } = string.Empty;
    public string ContextName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration { get; set; }
    public bool Succeeded { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}
