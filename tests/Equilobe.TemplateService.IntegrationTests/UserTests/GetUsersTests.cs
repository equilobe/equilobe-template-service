using Equilobe.TemplateService.IntegrationTests.Extensions;
using FluentAssertions;
using System.Net.Http.Json;
using NSubstitute;
using Equilobe.TemplateService.Mocks.Builders.Users;
using Equilobe.TemplateService.Core.Models;
using Equilobe.TemplateService.Core.Features.Users.GetUsers;
using Equilobe.TemplateService.Core.Common.Api;
using Equilobe.TemplateService.Core.Common.Extensions;

namespace Equilobe.TemplateService.IntegrationTests.UserTests;

public class GetUsersTests : BaseIntegrationTest
{
    private IntegrationTestWebAppFactory _factory;

    public GetUsersTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetUsers_ReturnsSuccessResult()
    {
        // Arrange

        var users = new List<UserEntity>
        {
            UserMocks.JohnDoe(),
            UserMocks.JaneDoe()
        };
        
        await DbContext.Users.AddRangeAsync(users);
        DateTimeProvider.GetUtcNow().Returns(DateTime.UtcNow);
        await DbContext.SaveChangesAsync();

        var client = _factory.CreateAuthenticatedClient(users.First().ExternalId);
        client.DefaultRequestHeaders.Add(TestAuthHandler.UserId, users.First().ExternalId);

        // Act
        var query = new GetUsersQuery
        {
            PageSize = 10,
            SortBy = "LastUpdatedAtUtc",
            SortDescending = true
        };
        var queryString = query.ToQueryString();
        var response = await client.GetAsync($"/api/users?{queryString}");

        // Assert
        var result = await response.Content.ReadFromJsonAsync<Page<GetUsersReadModel>>();

        result.Should().NotBeNull();
        result!.Items.Should().HaveCount(2);
        result!.TotalCount.Should().Be(2);
        result!.Items.First().FirstName.Should().Be("John");
        result!.Items.First().LastName.Should().Be("Doe");

        DbContext.CleanUpDatabase();
    }
}
