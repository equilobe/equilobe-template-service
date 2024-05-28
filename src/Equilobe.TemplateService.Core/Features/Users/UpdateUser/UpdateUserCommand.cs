using AutoMapper;
using Equilobe.TemplateService.Core.Common.Api;
using Equilobe.TemplateService.Core.Common.Exceptions;
using Equilobe.TemplateService.Core.Common.Interfaces;
using Equilobe.TemplateService.Core.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Equilobe.TemplateService.Core.Features.Users.UpdateUser;

public class UpdateUserCommand : IRequest
{
    [SwaggerIgnore]
    public long Id { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? ProfilePictureUrl { get; set; } = default!;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UpdateUserCommand, UserEntity>();
        }
    }
}

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty();

        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();
    }
}

public class UpdateUserCommandHandler(
    IDbContext dbContext, 
    IMapper mapper) : IRequestHandler<UpdateUserCommand>
{
    private readonly IDbContext dbContext = dbContext;
    private readonly IMapper mapper = mapper;

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken: cancellationToken);

        if (user == null)
        {
            throw new ResourceNotFoundException("User not found.");
        }

        mapper.Map(request, user);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
