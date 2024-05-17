namespace Equilobe.TemplateService.Core.Common.Exceptions;

public class AuthorizationException(string? message) : Exception(message)
{
    public AuthorizationException()
        : this(null) { }

    public static AuthorizationException Create(
        string? message = null)
    {
        return new AuthorizationException(message);
    }
}