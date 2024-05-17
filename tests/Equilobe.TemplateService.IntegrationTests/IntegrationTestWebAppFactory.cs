using Equilobe.TemplateService.IntegrationTests.Extensions;
using Equilobe.TemplateService.Core.Common.Interfaces;
using Equilobe.TemplateService.Infrastructure.Database;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.Net.Http.Headers;
using Testcontainers.PostgreSql;

namespace Equilobe.TemplateService.IntegrationTests;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public string DefaultUserId = "1";
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:14.1-alpine")
        .WithDatabase("local-equilobe-template-service-db")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTest");

        builder.ConfigureTestServices(services =>
        {
            RemoveServicesThatWillBeMocked(services);
            AddMockServices(services);

            SetAuth(services);
        });
    }

    public new HttpClient CreateClient()
    {
        var client = base.CreateClient();
        client.BaseAddress = new Uri("https://localhost/");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);

        return client;
    }

    public HttpClient CreateAuthenticatedClient(string externalUserId)
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Add(TestAuthHandler.UserId, externalUserId);
        return client;
    }

    private void SetAuth(IServiceCollection services)
    {
        services.Configure<TestAuthHandlerOptions>(options => options.DefaultUserId = DefaultUserId);

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
                options.DefaultScheme = TestAuthHandler.AuthenticationScheme;
                options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
            })
            .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(
            TestAuthHandler.AuthenticationScheme, options => { });
    }

    private static void RemoveServicesThatWillBeMocked(IServiceCollection services)
    {
        services.RemoveService<DbContextOptions<AppDbContext>>();
        services.RemoveService<IDateTimeProvider>();
    }

    private void AddMockServices(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = _dbContainer.GetConnectionString();
            options.UseNpgsql($"{connectionString};Include Error Detail=True");
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });

        var mockDateTimeProvider = Substitute.For<IDateTimeProvider>();
        services.AddSingleton(_ => mockDateTimeProvider);
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }
}
