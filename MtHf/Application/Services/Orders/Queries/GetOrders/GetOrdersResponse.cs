using Application.Common.Dto.Orders;

namespace Application.Services.Orders.Queries.GetOrders;

public record GetOrdersResponse(IReadOnlyList<OrderDto> Orders);
