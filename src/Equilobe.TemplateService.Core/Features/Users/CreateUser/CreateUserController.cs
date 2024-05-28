using Equilobe.TemplateService.Core.Common.Auth;
using Equilobe.TemplateService.Core.Common.Exceptions;
using Equilobe.TemplateService.Core.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Equilobe.TemplateService.Core.Features.Users.CreateUser;

[Route("api/users/me")]
[Authorize(Policy = PolicyNames.DefaultPolicy)]
public class CreateUserController(IMediator mediator, IClaimProvider claimProvider) : ControllerBase
{   
    private readonly IMediator mediator = mediator;
    private readonly IClaimProvider claimProvider = claimProvider;

    [HttpPost]
    [UserSwaggerOperation]
    public async Task<long> CreateUserAsync()
    {
        var command = new CreateUserCommand
        {
            Email = claimProvider.GetUserClaim(System.Security.Claims.ClaimTypes.Email) ?? throw new AuthorizationException("Email not found in claims."),
            FirstName = claimProvider.GetUserClaim(System.Security.Claims.ClaimTypes.GivenName) ?? throw new BadRequestException("First name not found in claims."),
            LastName = claimProvider.GetUserClaim(System.Security.Claims.ClaimTypes.Surname) ?? throw new BadRequestException("Last name not found in claims."),
            ExternalId = claimProvider.GetUserClaim(System.Security.Claims.ClaimTypes.NameIdentifier) ?? throw new AuthorizationException("User id not found in claims.")
        };

        return await mediator.Send(command);
    }
}

