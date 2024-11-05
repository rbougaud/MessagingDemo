using Domain.Abstraction.Customers;

namespace Domain.Abstraction.Orders;

public interface IOrderDto
{
    Guid Id { get; init; }
    DateOnly DateOrder { get; init; }
    DateOnly DueDate { get; init; }
    public short PaymentMode { get; init; }
    DateOnly DatePayment { get; init; }
    short DeliveryMode { get; init; }
    DateOnly DeliveryDate { get; init; }
    short State { get; init; }
    ICustomerDto CustomerDto { get; init; }
}
