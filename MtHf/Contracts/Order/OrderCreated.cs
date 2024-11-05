namespace Contracts.Order;

public record OrderCreated(Guid Id, DateOnly DateOrder, DateOnly DueDate, short DeliveryMode, short State, Guid CustomerId, Dictionary<Guid, int> OrderMovies);