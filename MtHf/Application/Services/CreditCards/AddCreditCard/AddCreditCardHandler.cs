using Application.Common.Mapping;
using Contracts.CreditCard;
using Domain.Abstraction;
using Domain.Abstraction.CreditCards;
using Domain.Abstraction.Customers;
using Domain.Common.Helper;
using MediatR;
using Serilog;

namespace Application.Services.CreditCards.AddCreditCard;

public class AddCreditCardHandler : IRequestHandler<AddCreditCardCommand, Result<AddCreditCardResponse, List<string>>>
{
    private readonly ILogger _logger;
    private readonly IEventStore _eventStore;
    private readonly ICustomerRepositoryReader _repositoryCustomer;
    private readonly ICreditCardRepositoryReader _repositoryCreditCardReader;
    private readonly ICreditCardRepositoryWriter _repositoryCreditCardWriter;

    public AddCreditCardHandler(ILogger logger, IEventStore eventStore, ICustomerRepositoryReader repositoryCustomer,
        ICreditCardRepositoryReader repositoryCreditCardReader, ICreditCardRepositoryWriter repositoryCreditCardWriter)
    {
        _logger = logger;
        _eventStore = eventStore;
        _repositoryCustomer = repositoryCustomer;
        _repositoryCreditCardReader = repositoryCreditCardReader;
        _repositoryCreditCardWriter = repositoryCreditCardWriter;
    }

    public async Task<Result<AddCreditCardResponse, List<string>>> Handle(AddCreditCardCommand request, CancellationToken cancellationToken)
    {
        _logger.Information(nameof(AddCreditCardHandler));
        var resustCustomer = await _repositoryCustomer.GetCustomerByIdAsync(request.CustomerId);
        if (resustCustomer.IsFailure || resustCustomer.Value is null)
        {
            return new List<string> { resustCustomer.Error.Message };
        }

        var resultexist = _repositoryCreditCardReader.CheckIfExist(request.CustomerId);
        if (resultexist.IsFailure)
        {
            return new List<string> { resultexist.Error.Message };
        }

        var @event = new CreditCardCreated(Ulid.NewUlid().ToGuid(), request.HolderName, request.CardType, request.CardNumber, request.ValidityDate, request.MvcCode, resustCustomer.Value.Id);
        var result = await _eventStore.SaveAsync(@event, cancellationToken);
        if (result.IsFailure)
        {
            return new List<string> { result.Error.Message };
        }
        var resultId = await _repositoryCreditCardWriter.AddAsync(@event.ToDao());
        return resultId.IsSuccess ? new AddCreditCardResponse(resultId.Value.ToString()) : new List<string> { resultId.Error.Message };
    }
}
