using AutoMapper;
using Equilobe.TemplateService.Core.Common.Interfaces;
using Equilobe.TemplateService.Core.Features.Users.GetUsers;
using Equilobe.TemplateService.Core.Models;
using Equilobe.TemplateService.Mocks.Builders.Users;
using FluentAssertions;
using MockQueryable.NSubstitute;
using NSubstitute;

namespace Equilobe.TemplateService.UnitTests.FeatureTests.Users;

public class GetUsersTests
{
    private readonly IDbContext _dbContextMock;
    private readonly IMapper _mapper;

    public GetUsersTests()
    {
        _dbContextMock = Substitute.For<IDbContext>();

        var usersSetMock = GetUsersSeed()
            .AsQueryable()
            .BuildMock()
            .BuildMockDbSet();

        _dbContextMock.Users.Returns(usersSetMock);
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<GetUsersMappingProfile>()).CreateMapper();
    }

    [Theory]
    [InlineData(2, "j")]
    [InlineData(1, "john")]
    [InlineData(3, null)]
    public async Task GetUsersQuery_BySearchText_ShouldReturnUsers(int expectedCount, string? searchText)
    {
        // Arrange
        var query = new GetUsersQuery { Text = searchText};

        // Act
        var result = await new GetUsersQueryHandler(_dbContextMock, _mapper)
            .Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TotalCount.Should().Be(expectedCount);
    }

    private List<UserEntity> GetUsersSeed()
    {
        return new List<UserEntity>
        {
            UserBuilder
                .Create()
                .WithId(1)
                .WithExternalId("1234567890")
                .WithFirstName("John")
                .WithLastName("Doe")
                .WithEmail("john_doe@gmail.com")
                .Build(),
            UserBuilder
                .Create()
                .WithId(2)
                .WithExternalId("0987654321")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .WithEmail("jane@gmail.com")
                .Build(),
            UserBuilder
                .Create()
                .WithId(3)
                .WithExternalId("09876543212")
                .WithFirstName("Super")
                .WithLastName("Admin")
                .WithEmail("super-admin@gmail.com")
                .Build()
        };
    }
}
