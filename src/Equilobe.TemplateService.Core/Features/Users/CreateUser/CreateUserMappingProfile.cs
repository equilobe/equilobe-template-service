using AutoMapper;
using Equilobe.TemplateService.Core.Models;

namespace Equilobe.TemplateService.Core.Features.Users.CreateUser;

public class CreateUserMappingProfile : Profile
{
    public CreateUserMappingProfile()
    {
        CreateMap<CreateUserCommand, UserEntity>();
    }
}
