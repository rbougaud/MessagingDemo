using Domain.Entities;

namespace Contracts.Audit;

public record AuditTrailWriterMessage(List<AuditEntry> AuditEntries);
