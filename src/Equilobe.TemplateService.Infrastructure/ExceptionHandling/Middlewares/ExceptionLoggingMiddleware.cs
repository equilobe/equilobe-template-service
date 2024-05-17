using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Equilobe.TemplateService.Infrastructure.ExceptionHandling.Middlewares;

public class ExceptionLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionLoggingMiddleware> _logger;
    private readonly Func<Exception, LogLevel> _configureLogLevel;

    public ExceptionLoggingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionLoggingMiddleware> logger,
        Func<Exception, LogLevel> configureLogLevel)
    {
        _next = next;
        _logger = logger;
        _configureLogLevel = configureLogLevel;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.Log(_configureLogLevel.Invoke(ex), ex.Demystify(), $"An error occured in the request {context.Request.Method}: {context.Request.Path}.");
            throw;
        }
    }
}
