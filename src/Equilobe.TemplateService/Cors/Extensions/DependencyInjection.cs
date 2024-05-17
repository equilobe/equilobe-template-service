using Equilobe.TemplateService.Cors;

namespace Equilobe.TemplateService.Startup.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: PolicyNames.AllowOrigin,
                    policy =>
                    {
                        var allowedCorsOrigins = configuration
                          .GetSection("CorsAllow")
                          .Get<List<string>>()?
                          .ToArray() ?? default!;

                        policy.WithOrigins(allowedCorsOrigins)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    }
                );
            });

            return services;
        }
    }
}

