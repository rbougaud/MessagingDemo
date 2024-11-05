namespace Contracts.Order;

public record OrderPaymentValidated(Guid Id, short PaymentMode ,short State);
