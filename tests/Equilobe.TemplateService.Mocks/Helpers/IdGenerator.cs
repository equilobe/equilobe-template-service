namespace Equilobe.TemplateService.Mocks.Helpers;

public class IdGenerator
{
    public static long GenerateId() => DateTime.UtcNow.Ticks + new Random().Next(1, 1000);
    public static string GenerateExternalId(string identityProvider = "auth0") => $"{identityProvider}|{GenerateId()}";
}
