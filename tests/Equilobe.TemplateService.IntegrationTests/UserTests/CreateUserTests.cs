using Equilobe.TemplateService.Mocks.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System.Net;
using System.Net.Http.Json;

namespace Equilobe.TemplateService.IntegrationTests.UserTests;

public class CreateUserTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    private readonly IntegrationTestWebAppFactory _factory = factory;

    [Fact]
    public async Task CreateUser_WhenUserDoesNotExist_ShouldCreateUser()
    {
        // Arrange
        var externalUserId = IdGenerator.GenerateExternalId();

        var httpClient = _factory.CreateClient();
        httpClient.DefaultRequestHeaders.Add(TestAuthHandler.UserId, externalUserId);
        httpClient.DefaultRequestHeaders.Add(TestAuthHandler.Email, "john_doe@gmail.com");
        httpClient.DefaultRequestHeaders.Add(TestAuthHandler.FirstName, "John");
        httpClient.DefaultRequestHeaders.Add(TestAuthHandler.LastName, "Doe");

        // Act
        var response = await httpClient.PostAsJsonAsync("/api/users/me", new { });

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var user = await DbContext
            .Users
            .FirstOrDefaultAsync(u => u.ExternalId == externalUserId);

        user.Should().NotBeNull();
        user?.FirstName.Should().Be("John");
        user?.LastName.Should().Be("Doe");
        user?.Email.Should().Be("john_doe@gmail.com");
        user?.ExternalId.Should().Be(externalUserId);
    }
}
