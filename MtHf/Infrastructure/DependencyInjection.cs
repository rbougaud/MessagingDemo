using Domain.Abstraction;
using Domain.Abstraction.CreditCards;
using Domain.Abstraction.Customers;
using Domain.Abstraction.LoyaltyAccounts;
using Domain.Abstraction.Movies;
using Domain.Abstraction.Orders;
using Domain.Entities;
using Hangfire;
using Infrastructure.Events;
using Infrastructure.Outbox;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<WriterContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Infrastructure")));
        services.AddDbContext<ReaderContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Infrastructure")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
        //Le storage permet de retrouver les flux en cas de rupture brutale du déroulement de l'application
        services.AddHangfire(config => config.UseSqlServerStorage(connectionString));
        services.AddHangfireServer();

        services.AddScoped<IEventStore, EventStore>();
        services.AddScoped<ICustomerRepositoryWriter, CustomerRepositoryWriter>();
        services.AddScoped<ICustomerRepositoryReader, CustomerRepositoryReader>();
        services.AddScoped<IMovieRepositoryWriter, MovieRepositoryWriter>();
        services.AddScoped<IMovieRepositoryReader, MovieRepositoryReader>();
        services.AddScoped<ILoyaltyAccountRepositoryWriter, LoyaltyAccountRepositoryWriter>();
        services.AddScoped<ILoyaltyAccountRepositoryReader, LoyaltyAccountRepositoryReader>();
        services.AddScoped<ICreditCardRepositoryWriter, CreditCardRepositoryWriter>();
        services.AddScoped<ICreditCardRepositoryReader, CreditCardRepositoryReader>();
        services.AddScoped<IOrderRepositoryReader, OrderRepositoryReader>();
        services.AddScoped<IOrderRepositoryWriter, OrderRepositoryWriter>();

        services.AddScoped<IProcessOutboxMessagesJob, OutboxProcessor>();

        services.AddKeyedScoped<List<AuditEntry>>("Audit", (_, _) => new());
        return services;
    }
}
