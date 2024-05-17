using System;
using Equilobe.TemplateService.Infrastructure.ExceptionHandling.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace Equilobe.TemplateService.Infrastructure.ExceptionHandling.Services;

public interface IExceptionHandlerInvoker
{
    ProblemDetails Handle(Exception exception);
}

public class ExceptionHandlerInvoker : IExceptionHandlerInvoker
{
    private readonly IServiceProvider _serviceProvider;

    public ExceptionHandlerInvoker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ProblemDetails Handle(Exception exception)
    {
        var descriptor = GetHandler(exception.GetType());
        if (descriptor == null)
            throw new Exception($"No exception handlers are registered for a type '{exception.GetType().Name}' or its base classes.");

        var methodName = nameof(IExceptionHandler<Exception>.CreateProblemDetailsFromException);
        var handleMethod = descriptor.HandlerType.GetMethod(methodName, new[] { exception.GetType() });

        return (ProblemDetails)(handleMethod?.Invoke(descriptor.Handler, new[] { exception }) ?? default!);
    }

    private HandlerDescriptor? GetHandler(Type? exceptionType)
    {
        if (exceptionType == null || exceptionType == typeof(object))
            return null;

        var closedHandlerType = typeof(IExceptionHandler<>).MakeGenericType(exceptionType);
        var handler = _serviceProvider.GetService(closedHandlerType);
        if (handler != null)
            return new HandlerDescriptor(handler, closedHandlerType);

        return GetHandler(exceptionType.BaseType);
    }
}

internal class HandlerDescriptor
{
    public HandlerDescriptor(object handler, Type handlerType)
    {
        Handler = handler;
        HandlerType = handlerType;
    }

    public object Handler { get; }

    public Type HandlerType { get; }
}

