
using Equilobe.TemplateService.Core.Models;

namespace Equilobe.TemplateService.Mocks.Builders.Users;

public class UserBuilder
{
    private readonly UserEntity _user;

    public static UserBuilder Create()
    {
        return new UserBuilder();
    }

    private UserBuilder()
    {
        _user = new UserEntity();
    }

    public UserBuilder WithId(long id)
    {
        _user.Id = id;
        return this;
    }

    public UserBuilder WithFirstName(string firstName)
    {
        _user.FirstName = firstName;
        return this;
    }

    public UserBuilder WithLastName(string lastName)
    {
        _user.LastName = lastName;
        return this;
    }

    public UserBuilder WithEmail(string email)
    {
        _user.Email = email;
        return this;
    }

    public UserBuilder WithExternalId(string externalId)
    {
        _user.ExternalId = externalId;
        return this;
    }

    public UserEntity Build()
    {
        return _user;
    }
}
