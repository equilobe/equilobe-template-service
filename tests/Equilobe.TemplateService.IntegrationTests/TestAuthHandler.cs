using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Equilobe.TemplateService.IntegrationTests;

public class TestAuthHandlerOptions : AuthenticationSchemeOptions
{
    public string DefaultUserId { get; set; } = null!;
}

public class TestAuthHandler : AuthenticationHandler<TestAuthHandlerOptions>
{
    public const string UserId = "UserId";
    public const string FirstName = "FirstName";
    public const string LastName = "LastName";
    public const string Email = "Email";
    public const string AuthenticationScheme = "Test";

    private readonly string _defaultUserId;

    public TestAuthHandler(
        IOptionsMonitor<TestAuthHandlerOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : base(options, logger, encoder)
    {
        _defaultUserId = options.CurrentValue.DefaultUserId;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Test user") };

        if (Context.Request.Headers.TryGetValue(UserId, out var userId))
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId[0]!));
        }
        else
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, _defaultUserId));
        }

        AddHeaderAsClaimIfExists(FirstName, ClaimTypes.GivenName);
        AddHeaderAsClaimIfExists(LastName, ClaimTypes.Surname);
        AddHeaderAsClaimIfExists(Email, ClaimTypes.Email);

        var identity = new ClaimsIdentity(claims, AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);

        void AddHeaderAsClaimIfExists(string headerName, string claimType)
        {
            if (Context.Request.Headers.TryGetValue(headerName, out var headerValue))
            {
                claims!.Add(new Claim(claimType, headerValue[0]!));
            }
        }
    }
}
