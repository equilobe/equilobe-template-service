using Serilog;

namespace Equilobe.TemplateService.Startup.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void Deconstruct(
        this WebApplicationBuilder builder,
        out WebApplicationBuilder b,
        out IServiceCollection services,
        out IConfiguration configuration)
    {
        b = builder;
        services = builder.Services;
        configuration = builder.Configuration;
    }

    public static void AddSerilogLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
    }
}
