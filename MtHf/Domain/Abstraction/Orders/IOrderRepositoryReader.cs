using Domain.Common.Helper;
using Domain.Entities.Projections;

namespace Domain.Abstraction.Orders;

public interface IOrderRepositoryReader
{
    Task<Result<OrderProjection, Exception>> GetByIdAsync(Guid id);
    Task<Result<IEnumerable<OrderProjection>, Exception>> GetAllAsync();
    Task<List<OrderProjection>> GetPendingOrders();
    Task<List<OrderProjection>> GetReqPendingOrders();
}
