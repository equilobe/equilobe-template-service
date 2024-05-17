
using Equilobe.TemplateService.Core.Common.Interfaces;
using Equilobe.TemplateService.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Equilobe.TemplateService.IntegrationTests;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly IServiceScope _scope;
    protected readonly AppDbContext DbContext;
    protected readonly IDateTimeProvider DateTimeProvider;

    public BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();

        DbContext = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
        DateTimeProvider = _scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();

        DateTimeProvider.GetUtcNow().Returns(DateTime.UtcNow);
    }
}
