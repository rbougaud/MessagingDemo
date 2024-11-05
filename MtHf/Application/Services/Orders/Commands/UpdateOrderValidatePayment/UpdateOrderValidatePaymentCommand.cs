using Domain.Common.Helper;
using MediatR;

namespace Application.Services.Orders.Commands.UpdateOrderValidatePayment;

public record UpdateOrderValidatePaymentCommand(Guid OrderId, short PaymentMode) : IRequest<Result<UpdateOrderValidatePaymentResponse, List<string>>>;
