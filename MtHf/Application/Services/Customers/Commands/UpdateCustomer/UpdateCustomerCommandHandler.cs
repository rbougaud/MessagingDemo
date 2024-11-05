using Application.Common.Mapping;
using Contracts.Customer;
using Domain.Abstraction;
using Domain.Abstraction.Customers;
using Domain.Common.Helper;
using MediatR;
using Serilog;

namespace Application.Services.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result<UpdateCustomerResponse, List<string>>>
{
    private readonly ILogger _logger;
    private readonly IEventStore _eventStore;
    private readonly ICustomerRepositoryWriter _repositoryCustomer;

    public UpdateCustomerCommandHandler(ILogger logger, IEventStore eventStore, ICustomerRepositoryWriter repositoryCustomer)
    {
        _logger = logger;
        _eventStore = eventStore;
        _repositoryCustomer = repositoryCustomer;
    }


    public async Task<Result<UpdateCustomerResponse, List<string>>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(UpdateCustomerCommandHandler));
        var @event = new CustomerUpdated(request.Id, request.FirstName, request.LastName, request.Mail, request.Address, request.Phone, request.Iban);
        var result = await _repositoryCustomer.UpdateAsync(@event.ToDao());
        if (result.IsSuccess)
        {
            result = await _eventStore.SaveAsync(@event, cancellationToken);
        }
        return result.IsSuccess ? new UpdateCustomerResponse(result.Value) : new List<string> { result.Error.Message };
    }
}
