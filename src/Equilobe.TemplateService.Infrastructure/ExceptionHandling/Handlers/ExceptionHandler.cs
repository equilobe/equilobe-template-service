using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Equilobe.TemplateService.Infrastructure.ExceptionHandling.Handlers;

public interface IExceptionHandler<TException> where TException : Exception
{
    ProblemDetails CreateProblemDetailsFromException(TException exception);
}

public class ExceptionHandler : IExceptionHandler<Exception>
{
    public ProblemDetails CreateProblemDetailsFromException(Exception exception)
    {
        return new ProblemDetails
        {
            Title = "Server error",
            Status = StatusCodes.Status500InternalServerError,
            Detail = "A server error occurred."
        };
    }
}