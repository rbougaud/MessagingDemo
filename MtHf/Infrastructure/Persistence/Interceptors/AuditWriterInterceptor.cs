using Contracts.Audit;
using Domain.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence.Interceptors;

public class AuditWriterInterceptor(List<AuditEntry> auditEntries, IPublishEndpoint publishEndpoint) : SaveChangesInterceptor
{
    private readonly List<AuditEntry> _auditEntries = auditEntries;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var startTime = DateTime.UtcNow;

        var auditEntries = eventData.Context.ChangeTracker
            .Entries()
            .Where(x => x.Entity is not AuditEntry
                    &&
                    x.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .Select(x => new AuditEntry
            {
                Id = Ulid.NewUlid().ToGuid(),
                StartTime = startTime,
                MetaData = x.DebugView.LongView
            }).ToList();

        if (auditEntries.Count == 0)
        {
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        _auditEntries.AddRange(auditEntries);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return await base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        var endTime = DateTime.UtcNow;

        foreach (var auditEntry in _auditEntries)
        {
            auditEntry.EndTime = endTime;
            auditEntry.Succeeded = true;
        }


        if (_auditEntries.Count > 0)
        {
            //var auditEntriesClone = new List<AuditEntry>(_auditEntries);
            //_auditEntries.Clear();
            await _publishEndpoint.Publish(new AuditTrailWriterMessage(_auditEntries), cancellationToken);
        }

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    public override async Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return;
        }

        var endTime = DateTime.UtcNow;

        foreach (var auditEntry in _auditEntries)
        {
            auditEntry.EndTime = endTime;
            auditEntry.Succeeded = false;
            auditEntry.ErrorMessage = eventData.Exception.Message;
        }

        if (_auditEntries.Count > 0)
        {
            await _publishEndpoint.Publish(new AuditTrailWriterMessage(_auditEntries), cancellationToken);
        }
    }
}
