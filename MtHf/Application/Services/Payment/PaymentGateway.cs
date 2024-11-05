using Domain.Abstraction;
using Domain.Abstraction.LoyaltyAccounts;
using Domain.Common.Constantes;
using Domain.Entities.Projections;
using Serilog;

namespace Application.Services.Payment;

public class PaymentGateway(ILogger logger, ILoyaltyAccountRepositoryWriter loyaltyAccountRepositoryWriter) : IPaymentGateway
{
    private readonly ILogger _logger = logger;
    private readonly ILoyaltyAccountRepositoryWriter _loyaltyAccountRepositoryWriter = loyaltyAccountRepositoryWriter;

    public bool ProcessCreditCardPayment(CreditCardProjection creditCard, double amount)
    {
        // Logique simulée pour traiter un paiement par carte de crédit
        string cardNumber = creditCard.CardNumber.Substring(creditCard.CardNumber.Length - 4);
        _logger.Information("Processing credit card payment for {amount} using card number ending in {cardNumber}.", amount, cardNumber);

        return SimulatePaymentResponse();
    }

    public bool ProcessIbanPayment(string iban, double amount)
    {
        // Logique simulée pour traiter un paiement via IBAN
        _logger.Information("Processing IBAN payment for {amount} using IBAN: {iban}.", amount, iban);

        return SimulatePaymentResponse();
    }

    public async Task<bool> ProcessLoyaltyPointsPayment(LoyaltyAccountProjection loyaltyAccount, double totalAmount)
    {
        _logger.Information("Processing points payment for {totalAmount} using loyalty account: {id}.", totalAmount, loyaltyAccount.Id);
        int remainedPoints = loyaltyAccount.Points - (int)(totalAmount / Const.RATE_POINTS);
        if (remainedPoints > 0)
        {
            await _loyaltyAccountRepositoryWriter.UpdatePointsAsync(loyaltyAccount);
            return true;
        }
        else
        {
            _logger.Information("Customer does not have enought points to buy the products");
            return false;
        }
    }

    /// <summary>
    /// Simuler une réponse de succès ou d'échec
    /// </summary>
    /// <returns></returns>
    private static bool SimulatePaymentResponse()
    {
        return Random.Shared.Next(0, 2) == 1;
    }
}
