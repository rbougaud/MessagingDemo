using Domain.Abstraction;
using Domain.Abstraction.Orders;
using Domain.Common.Constantes;
using Domain.Common.Enums;
using Domain.Entities.Projections;
using Infrastructure.Persistence.Context;
using Serilog;

namespace Application.Services.Payment;

public class PaymentService(ILogger logger, IPaymentGateway paymentGateway, IOrderRepositoryReader orderRepositoryReader, WriterContext writerContext) : IPaymentService
{
    private readonly ILogger _logger = logger;
    private readonly IPaymentGateway _paymentGateway = paymentGateway;
    private readonly IOrderRepositoryReader _orderRepositoryReader = orderRepositoryReader;
    private readonly WriterContext _writerContext = writerContext;

    public async Task PaymentProcessAsync()
    {
        _logger.Information(nameof(PaymentProcessAsync));
        List<OrderProjection> pendingOrders = await _orderRepositoryReader.GetPendingOrders();

        try
        {
            foreach (var order in pendingOrders)
            {
                if (order.Customer is not null)
                {
                    double totalAmount = CalculateOrderTotal(order);
                    bool paymentSuccess = await PaymentProcessAccordingMode(order.Customer, totalAmount, order.PaymentMode);

                    if (paymentSuccess)
                    {
                        _logger.Information("Payment succeed ,SendMail to {mail}", order.Customer.Mail);
                        order.State = (short)StateOrder.Paid;
                        order.DatePayment = DateOnly.FromDateTime(DateTime.Now);

                        var account = order.Customer.LoyaltyAccount;
                        if (account != null)
                        {
                            int points = (int)(totalAmount * Const.RATE_POINTS);
                            _logger.Information("The customer {firstname} {lastname} has won {points} points.", order.Customer.FirstName, order.Customer.LastName, points);
                            account.Points += points;
                            _writerContext.LoyaltyAccountProjections.Update(account);
                        }
                    }
                    else
                    {
                        _logger.Information("Payment has failed for the order {id}, SendMail to {mail}", order.Id, order.Customer.Mail);
                        order.State = (short)StateOrder.PaymentFailed;
                    }
                    _writerContext.OrderProjections.Update(order);
                    await _writerContext.SaveChangesAsync();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
        }
    }

    private async Task<bool> PaymentProcessAccordingMode(CustomerProjection customer, double totalAmount, short paymentMode)
    {
        bool paymentSuccess = false;
        if (customer.CreditCard != null && paymentMode == (short)PaymentMode.ByCreditCard)
        {
            paymentSuccess = _paymentGateway.ProcessCreditCardPayment(customer.CreditCard, totalAmount);
        }
        else if (!string.IsNullOrEmpty(customer.Iban) && paymentMode == (short)PaymentMode.ByBank)
        {
            paymentSuccess = _paymentGateway.ProcessIbanPayment(customer.Iban, totalAmount);
        }
        else if(customer.LoyaltyAccount != null && paymentMode == (short)PaymentMode.ByLoyaltyPoints)
        {
            paymentSuccess = await _paymentGateway.ProcessLoyaltyPointsPayment(customer.LoyaltyAccount, totalAmount);
        }
        else
        {
            _logger.Error("No valid payment method found for the customer");
        }
        return paymentSuccess;
    }

    private double CalculateOrderTotal(OrderProjection order)
    {
        _logger.Information(nameof(CalculateOrderTotal));
        double total = 0.0;
        foreach (var movieCommand in order.MovieCommands)
        {
            total += movieCommand.Movie.SalePrice * movieCommand.Quantity;
        }
        return total;
    }
}
