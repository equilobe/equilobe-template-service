using System;
using Equilobe.TemplateService.Core.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Equilobe.TemplateService.Infrastructure.Services.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IUserProvider, UserProvider>();
            services.AddSingleton<IClaimProvider, ClaimProvider>();

            return services;
        }
    }
}

