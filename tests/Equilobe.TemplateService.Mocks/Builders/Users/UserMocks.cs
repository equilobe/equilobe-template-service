using Equilobe.TemplateService.Core.Models;
using Equilobe.TemplateService.Mocks.Helpers;

namespace Equilobe.TemplateService.Mocks.Builders.Users;


public class UserMocks
{
    public static UserEntity JohnDoe()
    {
        return UserBuilder
            .Create()
            .WithId(IdGenerator.GenerateId())
            .WithExternalId(IdGenerator.GenerateExternalId())
            .WithFirstName("John")
            .WithLastName("Doe")
            .WithEmail("john_doe@gmail.com")
            .Build();
    }

    public static UserEntity JaneDoe()
    {
        return UserBuilder
            .Create()
            .WithId(IdGenerator.GenerateId())
            .WithExternalId(IdGenerator.GenerateExternalId())
            .WithFirstName("Jane")
            .WithLastName("Doe")
            .WithEmail("jane_doe@gmail.com")
            .Build();
    }
}
