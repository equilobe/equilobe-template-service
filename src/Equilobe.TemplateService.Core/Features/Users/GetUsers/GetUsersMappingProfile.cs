using AutoMapper;
using Equilobe.TemplateService.Core.Models;

namespace Equilobe.TemplateService.Core.Features.Users.GetUsers;

public class GetUsersMappingProfile : Profile
{
    public GetUsersMappingProfile()
    {
        CreateMap<UserEntity, GetUsersReadModel>();
    }
}