using AutoMapper;
using Equilobe.TemplateService.UnitTests.Extensions;
using Equilobe.TemplateService.Core.Common.Exceptions;
using Equilobe.TemplateService.Core.Common.Interfaces;
using Equilobe.TemplateService.Core.Features.Users.CreateUser;
using Equilobe.TemplateService.Core.Models;
using FluentAssertions;
using MediatR;
using MockQueryable.NSubstitute;
using NSubstitute;

namespace Equilobe.TemplateService.UnitTests.FeatureTests.Users;

public class CreateUserTests
{
    private readonly IMediator _mediatorMock;
    private readonly IDbContext _dbContextMock;

    public CreateUserTests()
    {
        _mediatorMock = Substitute.For<IMediator>();
        _dbContextMock = Substitute.For<IDbContext>();
    }

    [Fact]
    public async Task CreateUsers_ReturnsSuccess()
    {
        // Arrange
        var usersSetMock = new List<UserEntity>()
            .AsQueryable()
            .BuildMock()
            .BuildMockDbSet();

        _dbContextMock.Users.Returns(usersSetMock);

        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<CreateUserMappingProfile>()).CreateMapper();

        var commandHandler = new CreateUserCommandHandler(
            _dbContextMock,
            mapper,
            _mediatorMock);

        var command = new CreateUserCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@gmail.com",
            ExternalId = "1234567890",
            ProfilePictureUrl = "https://www.google.com"
        };

        // Act
        var result = await commandHandler.Handle(command, CancellationToken.None);

        // Assert

        await _dbContextMock.ReceivedSavedChangesAsync();

        await _dbContextMock
            .Users
            .Received(1)
            .AddAsync(Arg.Is<UserEntity>(user => user.Id == 0 && user.Email == "john.doe@gmail.com"), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task TryCreateUserThatAlreadyExists_ThrowsDuplicateResourceException()
    {
        // Arrange
        var usersSetMock = new List<UserEntity>
        {
            new() 
            {
                Id = 12,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com",
                ExternalId = "1234567890",
            }
        }
        .AsQueryable()
        .BuildMock()
        .BuildMockDbSet();

        _dbContextMock.Users.Returns(usersSetMock);

        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<CreateUserMappingProfile>()).CreateMapper();

        var commandHandler = new CreateUserCommandHandler(
            _dbContextMock,
            mapper,
            _mediatorMock);

        var command = new CreateUserCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@gmail.com",
            ExternalId = "1234567890",
            ProfilePictureUrl = "https://www.google.com"
        };

        // Act
        Func<Task> act = async () => await commandHandler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DuplicateResourceException>();
    }
}
