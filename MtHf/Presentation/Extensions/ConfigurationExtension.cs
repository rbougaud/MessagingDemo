using Application.Consumers.Audit;
using Application.Consumers.CreditCards;
using Application.Consumers.Customers;
using Application.Consumers.LoyaltyAccounts;
using Application.Consumers.Movies;
using Application.Consumers.Orders;
using MassTransit;

namespace Presentation.Extensions;

internal static class ConfigurationExtension
{
    internal static void ConfigurationMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.AddConsumer<CustomerCreatedConsumer>();
            x.AddConsumer<CustomerUpdatedConsumer>();
            x.AddConsumer<CustomerDeletedConsumer>();

            x.AddConsumer<MovieCreatedConsumer>();
            x.AddConsumer<MovieDeletedConsumer>();
            x.AddConsumer<MovieUpdatedConsumer>();

            x.AddConsumer<LoyaltyAccountCreatedConsumer>();
            x.AddConsumer<LoyaltyAccountDeletedConsumer>();

            x.AddConsumer<CreditCardCreatedConsumer>();
            x.AddConsumer<CreditCardDeletedConsumer>();

            x.AddConsumer<OrderCreatedConsumer>();
            x.AddConsumer<OrderUpdateDeliveryConsumer>();
            x.AddConsumer<OrderPaymentValidatedConsumer>();
            x.AddConsumer<DeleteOrderByIdConsumer>();

            x.AddConsumer<AuditTrailWriterConsumer>();
            x.AddConsumer<AuditTrailReaderConsumer>();
            //TODO RBO voir si changement vers Hangfire
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}
