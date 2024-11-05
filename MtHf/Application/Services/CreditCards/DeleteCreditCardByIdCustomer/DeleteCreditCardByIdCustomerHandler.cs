using Application.Services.LoyalAccounts.DeleteLoyaltyAccountByIdCustomer;
using Application.Services.Movies.Commands.DeleteMovieByTitle;
using Contracts.CreditCard;
using Contracts.LoyaltyAccount;
using Domain.Abstraction;
using Domain.Abstraction.CreditCards;
using Domain.Common.Helper;
using MediatR;
using Serilog;

namespace Application.Services.CreditCards.DeleteCreditCardByIdCustomer;

public class DeleteCreditCardByIdCustomerHandler : IRequestHandler<DeleteCreditCardByIdCustomerCommand, Result<DeleteCreditCardByIdCustomerResponse, List<string>>>
{
    private readonly ILogger _logger;
    private readonly IEventStore _eventStore;
    private readonly ICreditCardRepositoryWriter _repositoryCreditCardWriter;

    public DeleteCreditCardByIdCustomerHandler(ILogger logger, IEventStore eventStore, ICreditCardRepositoryWriter creditCardRepositoryWriter)
    {
        _logger = logger;
        _eventStore = eventStore;
        _repositoryCreditCardWriter = creditCardRepositoryWriter;
    }

    public async Task<Result<DeleteCreditCardByIdCustomerResponse, List<string>>> Handle(DeleteCreditCardByIdCustomerCommand request, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(DeleteCreditCardByIdCustomerHandler));
        var result = await _repositoryCreditCardWriter.DeleteByCustomerIdAsync(request.CustomerId);
        if (result.IsSuccess && result.Value)
        {
            var @event = new CreditCardDeleted(Ulid.NewUlid().ToGuid(), request.CustomerId);
            result = await _eventStore.SaveAsync(@event, cancellationToken);
            return result.IsFailure ? new List<string> { result.Error.Message } : new DeleteCreditCardByIdCustomerResponse(true);
        }
        return result.IsSuccess ? new DeleteCreditCardByIdCustomerResponse(result.Value) : new List<string> { result.Error.Message };
    }
}
