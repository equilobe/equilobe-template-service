using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Equilobe.TemplateService.Infrastructure.Mediator.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(assemblies));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UserParametersBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UserRequestValidationBehaviour<,>));

        return services;
    }
}