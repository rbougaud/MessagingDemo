using Application.Common.Mapping;
using Contracts.LoyalAccount;
using Domain.Abstraction;
using Domain.Abstraction.Customers;
using Domain.Abstraction.LoyaltyAccounts;
using Domain.Common.Helper;
using MediatR;
using Serilog;

namespace Application.Services.LoyalAccounts.AddLoyalAccount;

public class AddLoyaltyAccountHandler : IRequestHandler<AddLoyaltyAccountCommand, Result<AddLoyaltyAccountResponse, List<string>>>
{
    private readonly ILogger _logger;
    private readonly IEventStore _eventStore;
    private readonly ICustomerRepositoryReader _repositoryCustomer;
    private readonly ILoyaltyAccountRepositoryWriter _repositoryLoyalAccountWriter;
    private readonly ILoyaltyAccountRepositoryReader _repositoryLoyaltyAccountReader;

    public AddLoyaltyAccountHandler(ILogger logger, IEventStore eventStore, ICustomerRepositoryReader repositoryCustomer,
        ILoyaltyAccountRepositoryWriter loyalAccountRepositoryWriter, ILoyaltyAccountRepositoryReader repositoryLoyaltyAccountReader)
    {
        _logger = logger;
        _eventStore = eventStore;
        _repositoryCustomer = repositoryCustomer;
        _repositoryLoyalAccountWriter = loyalAccountRepositoryWriter;
        _repositoryLoyaltyAccountReader = repositoryLoyaltyAccountReader;
    }

    public async Task<Result<AddLoyaltyAccountResponse, List<string>>> Handle(AddLoyaltyAccountCommand request, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(AddLoyaltyAccountHandler));
        var resustCustomer = await _repositoryCustomer.GetCustomerByFullNameAsync(request.FirstName, request.LastName);
        if (resustCustomer.IsFailure || resustCustomer.Value is null)
        {
            return new List<string> { resustCustomer.Error.Message };
        }

        var resultexist = await _repositoryLoyaltyAccountReader.GetByCustomerId(resustCustomer.Value.Id);
        if (resultexist.IsFailure)
        {
            return new List<string> { resultexist.Error.Message };
        }

        if (resultexist.Value!.IsDeleted)
        {
            var @event = new LoyaltyAccountCreated(resultexist.Value.Id, 0, resustCustomer.Value.Id);
            var result = await _eventStore.SaveAsync(@event, cancellationToken);
            if (result.IsFailure)
            {
                return new List<string> { result.Error.Message };
            }
            var resultId = await _repositoryLoyalAccountWriter.UndoDeletedAsync(resultexist.Value);
            return resultId.IsSuccess ? new AddLoyaltyAccountResponse(resultId.Value.ToString()) : new List<string> { resultId.Error.Message };
        }
        else
        {
            var @event = new LoyaltyAccountCreated(Ulid.NewUlid().ToGuid(), 0, resustCustomer.Value.Id);
            var result = await _eventStore.SaveAsync(@event, cancellationToken);
            if (result.IsFailure)
            {
                return new List<string> { result.Error.Message };
            }
            var resultId = await _repositoryLoyalAccountWriter.AddAsync(@event.ToDao());
            return resultId.IsSuccess ? new AddLoyaltyAccountResponse(resultId.Value.ToString()) : new List<string> { resultId.Error.Message };
        }
    }
}
