using Equilobe.TemplateService.Core.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Equilobe.TemplateService.Infrastructure.ExceptionHandling.Handlers;

public class BadRequestExceptionHandler : IExceptionHandler<BadRequestException>
{
    public ProblemDetails CreateProblemDetailsFromException(BadRequestException exception)
    {
        return new ProblemDetails
        {
            Title = "Bad request",
            Status = StatusCodes.Status400BadRequest,
            Detail = exception.Message
        };
    }
}