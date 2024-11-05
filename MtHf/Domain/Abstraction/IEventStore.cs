using Domain.Common.Helper;

namespace Domain.Abstraction;

public interface IEventStore
{
    Task<Result<bool, Exception>> SaveAsync<T>(T @event, CancellationToken cancellationToken) where T : class;
}
