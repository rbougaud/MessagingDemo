namespace Domain.Abstraction;

public interface IProcessOutboxMessagesJob
{
    Task ProcessAsync();
}
