using Equilobe.TemplateService.Core.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Equilobe.TemplateService.Infrastructure.ExceptionHandling.Handlers
{
    public class AuthorizationExceptionHandler : IExceptionHandler<AuthorizationException>
    {
        public ProblemDetails CreateProblemDetailsFromException(AuthorizationException exception)
        {
            return new ProblemDetails
            {
                Title = "Unauthorized",
                Status = StatusCodes.Status401Unauthorized,
                Detail = "You are not authorized to perform this action."
            };
        }
    }
}