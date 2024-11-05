namespace Contracts.Order;

public record OrderDeliveryOptionsUpdated(Guid Id, short State, short DeliveryMode, DateOnly DeliveryDate);
