using Application.Common.Behaviors;
using Application.Services.Payment;
using Domain.Abstraction;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
            services.AddValidatorsFromAssembly(assembly);
        }
        services.AddScoped<IPaymentGateway, PaymentGateway>();
        services.AddScoped<IPaymentService, PaymentService>();
        return services;
    }
}
