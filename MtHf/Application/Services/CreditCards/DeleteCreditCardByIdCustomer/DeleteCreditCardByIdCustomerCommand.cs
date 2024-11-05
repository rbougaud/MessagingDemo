using Domain.Common.Helper;
using MediatR;

namespace Application.Services.CreditCards.DeleteCreditCardByIdCustomer;

public readonly record struct DeleteCreditCardByIdCustomerCommand(Guid CustomerId) : IRequest<Result<DeleteCreditCardByIdCustomerResponse, List<string>>>;
