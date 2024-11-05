using Contracts.Customer;
using Domain.Abstraction;
using Domain.Abstraction.Customers;
using Domain.Common.Helper;
using MediatR;
using Serilog;

namespace Application.Services.Customers.Commands.DeleteCustomerByFullName;

public class DeleteCustomerByFullNameCommandHandler : IRequestHandler<DeleteCustomerByFullNameCommand, Result<DeleteCustomerByFullNameResponse, List<string>>>
{
    private readonly ILogger _logger;
    private readonly IEventStore _eventStore;
    private readonly ICustomerRepositoryWriter _repositoryCustomer;

    public DeleteCustomerByFullNameCommandHandler(ILogger logger, IEventStore eventStore, ICustomerRepositoryWriter repositoryCustomer)
    {
        _logger = logger;
        _eventStore = eventStore;
        _repositoryCustomer = repositoryCustomer;
    }

    public async Task<Result<DeleteCustomerByFullNameResponse, List<string>>> Handle(DeleteCustomerByFullNameCommand request, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(DeleteCustomerByFullNameCommandHandler));
        var result = await _repositoryCustomer.DeleteAsync(request.FirstName, request.LastName);
        if (result.IsSuccess && result.Value)
        {
            var @event = new CustomerDeleted(request.Id, request.FirstName, request.LastName);
            result = await _eventStore.SaveAsync(@event, cancellationToken);
            return result.IsFailure ? new List<string> { result.Error.Message } : new DeleteCustomerByFullNameResponse(true);
        }
        return result.IsSuccess ? new DeleteCustomerByFullNameResponse(result.Value) : new List<string> { result.Error.Message };
    }
}
