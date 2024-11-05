using Application.Common.Dto.MoviesCommand;
using Domain.Abstraction.Customers;
using Domain.Abstraction.Orders;

namespace Application.Common.Dto.Orders;

public record OrderDto : IOrderDto
{
    public Guid Id { get; init; }
    public DateOnly DateOrder { get; init; }
    public DateOnly DueDate { get; init; }
    public short PaymentMode { get; init; }
    public DateOnly DatePayment { get; init; }
    public short DeliveryMode { get; init; }
    public DateOnly DeliveryDate { get; init; }
    public short State { get; init; }
    public required ICustomerDto CustomerDto { get; init; }

    public ICollection<MovieCommandFullInfoDto> MovieCommandDtos { get; set; } = [];
}
