using FixNow.Application.Common.Behaviors;

using FluentValidation;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddPipelineBehaviors();

        return services;
    }

    private static IServiceCollection AddPipelineBehaviors(
        this IServiceCollection services)
    {
        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(UnhandledExceptionBehavior<,>));

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(LoggingBehavior<,>));

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(PerformanceBehavior<,>));

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(TransactionBehavior<,>));

        return services;
    }
}