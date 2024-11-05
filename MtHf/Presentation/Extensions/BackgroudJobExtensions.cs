using Domain.Abstraction;
using Hangfire;

namespace Presentation.Extensions;

public static class BackgroudJobExtensions
{
    public static IApplicationBuilder UseBackgroundJobs(this WebApplication app)
    {
        string schedule = app.Configuration["BackgroundJobs:Schedule"]!;
        app.Services.GetRequiredService<IRecurringJobManager>().AddOrUpdate<IProcessOutboxMessagesJob>("outbox-processor", job => job.ProcessAsync(), schedule);
        schedule = app.Configuration["BackgroundJobs:SchedulePayment"]!;
        app.Services.GetRequiredService<IRecurringJobManager>().AddOrUpdate<IPaymentService>("payment-processor", job => job.PaymentProcessAsync(), schedule);
        return app;
    }
}
