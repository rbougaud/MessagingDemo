using Domain.Entities;

namespace Contracts.Audit;

public record AuditTrailReaderMessage(AuditReadEntry AuditReadEntry);