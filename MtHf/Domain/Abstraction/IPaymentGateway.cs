using Domain.Entities.Projections;

namespace Domain.Abstraction;

public interface IPaymentGateway
{
    bool ProcessCreditCardPayment(CreditCardProjection creditCard, double amount);
    bool ProcessIbanPayment(string iban, double amount);
    Task<bool> ProcessLoyaltyPointsPayment(LoyaltyAccountProjection loyaltyAccount, double totalAmount);
}
