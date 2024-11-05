using Domain.Common.Enums;
using Domain.Common.Helper;
using Domain.Entities;

namespace Domain.Abstraction.Orders;

public interface IOrderRepositoryWriter
{
    Task<Result<Guid, Exception>> AddAsync(Order order);
    Task<Result<bool, Exception>> DeleteByIdAsync(Guid orderId);
    Task<Result<bool, Exception>> UpdateAsync(Order order);
    Task<Result<bool, Exception>> UpdateDeliveryAsync(Guid id, short state, short deliveryMode, DateOnly deliveryDate);
    Task<Result<bool, Exception>> UpdateStateAsync(Guid orderId, StateOrder state, short paymentMode);
}
