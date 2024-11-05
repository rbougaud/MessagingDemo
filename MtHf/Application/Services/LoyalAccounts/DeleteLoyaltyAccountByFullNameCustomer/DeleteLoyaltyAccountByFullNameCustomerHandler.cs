using Contracts.LoyaltyAccount;
using Domain.Abstraction;
using Domain.Abstraction.LoyaltyAccounts;
using Domain.Common.Helper;
using MediatR;
using Serilog;

namespace Application.Services.LoyalAccounts.DeleteLoyaltyAccountByIdCustomer;

public class DeleteLoyaltyAccountByFullNameCustomerHandler : IRequestHandler<DeleteLoyaltyAccountByFullNameCustomerCommand, Result<DeleteLoyaltyAccountByFullNameCustomerResponse, List<string>>>
{
    private readonly ILogger _logger;
    private readonly IEventStore _eventStore;
    private readonly ILoyaltyAccountRepositoryWriter _repositoryLoyalAccountWriter;

    public DeleteLoyaltyAccountByFullNameCustomerHandler(ILogger logger, IEventStore eventStore, ILoyaltyAccountRepositoryWriter loyaltyAccountRepositoryWriter)
    {
        _logger = logger;
        _eventStore = eventStore;
        _repositoryLoyalAccountWriter = loyaltyAccountRepositoryWriter;
    }

    public async Task<Result<DeleteLoyaltyAccountByFullNameCustomerResponse, List<string>>> Handle(DeleteLoyaltyAccountByFullNameCustomerCommand request, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(DeleteLoyaltyAccountByFullNameCustomerHandler));
        var result = await _repositoryLoyalAccountWriter.DeleteByCustomerFullNameAsync(request.FirstName, request.LastName);
        if (result.IsSuccess && result.Value)
        {
            var @event = new LoyaltyAccountDeleted(Ulid.NewUlid().ToGuid(), request.FirstName, request.LastName);
            result = await _eventStore.SaveAsync(@event, cancellationToken);
            return result.IsFailure ? new List<string> { result.Error.Message } : new DeleteLoyaltyAccountByFullNameCustomerResponse(true);
        }
        return result.IsSuccess ? new DeleteLoyaltyAccountByFullNameCustomerResponse(result.Value) : new List<string> { result.Error.Message };
    }
}
