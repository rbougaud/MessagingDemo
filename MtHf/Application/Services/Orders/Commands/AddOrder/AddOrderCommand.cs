using Domain.Common.Helper;
using MediatR;

namespace Application.Services.Orders.Commands.AddOrder;

public record AddOrderCommand(string FirstName, string LastName, string Mail, string? Address, string? Phone, Dictionary<Guid, int> OrderMovies) : IRequest<Result<AddOrderResponse, List<string>>>;
