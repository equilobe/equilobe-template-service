using System;
using Equilobe.TemplateService.Infrastructure.ExceptionHandling.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace Equilobe.TemplateService.Infrastructure.ExceptionHandling.Extensions;

public static class WebApplicationExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        return app;
    }


    public static IApplicationBuilder UseExceptionLogging(this IApplicationBuilder app, Func<Exception, LogLevel>? configureLogLevel = null)
    {
        if (configureLogLevel == null)
            configureLogLevel = _ => LogLevel.Error;

        app.UseMiddleware<ExceptionLoggingMiddleware>(configureLogLevel);

        return app;
    }
}
