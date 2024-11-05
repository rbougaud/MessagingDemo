using Application.Common.Mapping;
using Contracts.Customer;
using Domain.Abstraction;
using Domain.Abstraction.Customers;
using Domain.Common.Helper;
using MediatR;
using Serilog;

namespace Application.Services.Customers.Commands.AddCustomer;

public class AddCustomerCommandHandler : IRequestHandler<AddCustomerCommand, Result<AddCustomerResponse, List<string>>>
{
    private readonly ILogger _logger;
    private readonly IEventStore _eventStore;
    private readonly ICustomerRepositoryWriter _repositoryCustomer;

    public AddCustomerCommandHandler(ILogger logger, IEventStore eventStore, ICustomerRepositoryWriter repositoryCustomer)
    {
        _logger = logger;
        _eventStore = eventStore;
        _repositoryCustomer = repositoryCustomer;
    }

    public async Task<Result<AddCustomerResponse, List<string>>> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(AddCustomerCommandHandler));
        var resultExist = _repositoryCustomer.CheckCustomerExist(request.FirstName, request.LastName);
        if (resultExist.IsFailure)
        {
            return new List<string> { resultExist.Error.Message };
        }

        var @event = new CustomerCreated(Ulid.NewUlid().ToGuid(), request.FirstName, request.LastName, request.Mail, request.Address, request.Phone, request.Iban);
        var result = await _eventStore.SaveAsync(@event, cancellationToken);
        if (result.IsFailure)
        {
            return new List<string> { result.Error.Message };
        }
        var resultId = await _repositoryCustomer.AddAsync(@event.ToDao());
        return resultId.IsSuccess ? new AddCustomerResponse(resultId.Value.ToString()) : new List<string> { resultId.Error.Message };
    }
}
