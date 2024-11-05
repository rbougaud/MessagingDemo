using Domain.Entities;
using Domain.Entities.Projections;
using Infrastructure.Configuration;
using Infrastructure.Configuration.Projections;
using Infrastructure.Persistence.Interceptors;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Context;

public class WriterContext : DbContext
{
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CreditCard> CreditCards { get; set; }
    public DbSet<LoyaltyAccount> LoyaltyAccounts { get; set; }
    public DbSet<MovieCommand> MovieCommands { get; set; }

    public DbSet<EventStoreEntry> EventStore { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    
    public DbSet<CustomerProjection> CustomerProjections { get; set; }
    public DbSet<MovieProjection> MovieProjections { get; set; }
    public DbSet<LoyaltyAccountProjection> LoyaltyAccountProjections { get; set; }
    public DbSet<CreditCardProjection> CreditCardProjections { get; set; }
    public DbSet<OrderProjection> OrderProjections { get; set; }
    public DbSet<MovieCommandProjection> MovieCommandProjections { get; set; }

    public DbSet<AuditEntry> AuditEntries { get; set; }
    public DbSet<AuditReadEntry> AuditReadEntries { get; set; }

    private readonly List<AuditEntry> _auditEntries = [];
    private readonly IPublishEndpoint _publishEndpoint;

    public WriterContext(DbContextOptions<WriterContext> options, [FromKeyedServices("Audit")] List<AuditEntry> auditEntries, IPublishEndpoint publishEndpoint) : base(options)
    {
        _auditEntries = auditEntries;
        _publishEndpoint = publishEndpoint;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new LoyaltyAccountConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new CreditCardConfiguration());
        modelBuilder.ApplyConfiguration(new MovieConfiguration());
        modelBuilder.ApplyConfiguration(new MovieCommandConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerProjectionConfiguration());
        modelBuilder.ApplyConfiguration(new MovieProjectionConfiguration());
        modelBuilder.ApplyConfiguration(new LoyaltyAccountProjectionConfiguration());
        modelBuilder.ApplyConfiguration(new CreditCardProjectionConfiguration());
        modelBuilder.ApplyConfiguration(new OrderProjectionConfiguration());
        modelBuilder.ApplyConfiguration(new MovieCommandProjectionConfiguration());
        modelBuilder.ApplyConfiguration(new AuditEntryConfiguration());
        modelBuilder.ApplyConfiguration(new AuditReadEntryConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new AuditWriterInterceptor(_auditEntries, _publishEndpoint));
        base.OnConfiguring(optionsBuilder);
    }
}
