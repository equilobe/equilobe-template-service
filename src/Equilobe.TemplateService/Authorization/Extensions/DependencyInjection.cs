using Equilobe.TemplateService.Core.Common.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Equilobe.TemplateService.Authorization.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddOwnAuthorization(this IServiceCollection services)
    {
        services.AddAuthorizationCore(options =>
        {
            options.AddPolicy(PolicyNames.DefaultPolicy, po => po.RequireAuthenticatedUser());

            options.DefaultPolicy = options.GetPolicy(PolicyNames.DefaultPolicy)!;
        });

        return services;
    }
}

