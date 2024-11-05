using Contracts.Audit;
using Domain.Entities;
using Infrastructure.Common;
using MassTransit;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace Infrastructure.Persistence.Interceptors;

public class AuditReaderInterceptor(IPublishEndpoint publishEndpoint) : DbCommandInterceptor
{
    private readonly string _userId = UserContext.CurrentUserId;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
    private static readonly Lazy<List<AuditReadEntry>> _lazyAuditEntries = new([]);

    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
        try
        {
            var auditEntry = new AuditReadEntry
            {
                Id = Ulid.NewUlid().ToGuid(),
                UserId = _userId,
                QueryText = command.CommandText,
                StartTime = DateTime.UtcNow,
                ContextName = eventData.Context?.GetType().Name ?? string.Empty
            };
            _lazyAuditEntries.Value.Add(auditEntry);
            return base.ReaderExecuting(command, eventData, result);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public override async ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_lazyAuditEntries.Value?.Count > 0)
            {
                var entries = _lazyAuditEntries.Value.ToList(); // Créer une copie pour éviter la modification pendant l'itération
                foreach (var auditEntry in entries)
                {
                    auditEntry.EndTime = DateTime.UtcNow;
                    auditEntry.Duration = auditEntry.EndTime - auditEntry.StartTime;
                    await _publishEndpoint.Publish(new AuditTrailReaderMessage(auditEntry), cancellationToken);
                }
                _lazyAuditEntries.Value.Clear();
            }
            return await base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
        }
        catch (Exception ex)
        {
            throw;
        }
    }


}
