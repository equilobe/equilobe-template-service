using Equilobe.TemplateService.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Equilobe.TemplateService.Startup.Extensions;

public static class WebApplicationExtensions
{
    public static void Deconstruct(
        this WebApplication application,
        out IApplicationBuilder middleware,
        out IEndpointRouteBuilder endpoints,
        out WebApplication app)
    {
        middleware = application;
        endpoints = application;
        app = application;
    }

    public static void ApplyMigrations(this WebApplication application)
    {
        using var scope = application.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }
}