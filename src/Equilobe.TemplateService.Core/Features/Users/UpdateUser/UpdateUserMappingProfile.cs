using AutoMapper;
using Equilobe.TemplateService.Core.Models;

namespace Equilobe.TemplateService.Core.Features.Users.UpdateUser
{
    public class UpdateUserMappingProfile : Profile
    {
        public UpdateUserMappingProfile()
        {
            CreateMap<UpdateUserCommand, UserEntity>();
            CreateMap<UserEntity, UpdateUserCommand>();
        }
    }
}
