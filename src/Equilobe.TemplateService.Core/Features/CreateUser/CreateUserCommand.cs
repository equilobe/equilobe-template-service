using System;
using AutoMapper;
using Equilobe.TemplateService.Core.Common.Api;
using Equilobe.TemplateService.Core.Common.Exceptions;
using Equilobe.TemplateService.Core.Common.Interfaces;
using Equilobe.TemplateService.Core.DomainEvents;
using Equilobe.TemplateService.Core.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Equilobe.TemplateService.Core.Features.Users.CreateUser
{
    public class CreateUserCommand : IRequest<long>
    {
        [SwaggerIgnore]
        public long Id { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        [SwaggerIgnore]
        public string ExternalId { get; set; } = default!;
        public string? ProfilePictureUrl { get; set; } = default!;
    }

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty();

            RuleFor(x => x.ExternalId)
                .NotEmpty();

            RuleFor(x => x.FirstName)
                .NotEmpty();

            RuleFor(x => x.LastName)
                .NotEmpty();
        }
    }

    public class CreateUserCommandHandler(IDbContext dbContext, IMapper mapper, IMediator mediator) : IRequestHandler<CreateUserCommand, long>
    {
        private readonly IDbContext dbContext = dbContext;
        private readonly IMapper mapper = mapper;
        private readonly IMediator mediator = mediator;

        public async Task<long> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.ExternalId == request.ExternalId, cancellationToken: cancellationToken);

            if (existingUser != null)
            {
                throw DuplicateResourceException.Create<UserEntity>();
            }

            var user = mapper.Map<UserEntity>(request);

            await dbContext.Users.AddAsync(user, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            await mediator.Publish(new UserCreatedEvent(
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.ExternalId,
                user.ProfilePictureUrl
            ), cancellationToken);

            return user.Id;
        }
    }
}

