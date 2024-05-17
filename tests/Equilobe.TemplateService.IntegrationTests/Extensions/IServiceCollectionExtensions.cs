using Microsoft.Extensions.DependencyInjection;

namespace Equilobe.TemplateService.IntegrationTests.Extensions;

public static class IServiceCollectionExtensions
{
    public static void RemoveService<T>(this IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(
            d => d.ServiceType ==
                typeof(T));

        if (descriptor != null)
        {
            services.Remove(descriptor);
        }
    }
}