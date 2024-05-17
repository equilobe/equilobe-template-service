namespace Equilobe.TemplateService.Core.Common.Interfaces;

public interface IClaimProvider
{
    string? GetUserClaim(string claimType);
}