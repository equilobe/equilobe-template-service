using System;
using AutoMapper;
using Equilobe.TemplateService.Core.Common.Api;
using Equilobe.TemplateService.Core.Common.Extensions;
using Equilobe.TemplateService.Core.Common.Interfaces;
using FluentValidation;
using MediatR;

namespace Equilobe.TemplateService.Core.Features.Users.GetUsers
{
    public class GetUsersQuery :
        PageAndSortFilter,
        IRequest<IPage<GetUsersReadModel>>
    {
        public string? Text { get; set; }
        public string? ExternalId { get; set; }
    }

    public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
    {
        public GetUsersQueryValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(50);
        }
    }

    public class GetUsersQueryHandler(IDbContext dbContext, IMapper mapper) : IRequestHandler<GetUsersQuery, IPage<GetUsersReadModel>>
    {
        private readonly IDbContext dbContext = dbContext;
        private readonly IMapper mapper = mapper;

        public async Task<IPage<GetUsersReadModel>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var query = dbContext
                .Users
                .AsQueryable();

            if (request.ExternalId is not null)
            {
                query = query.Where(u => u.ExternalId == request.ExternalId);
            }

            if (request.Text is not null)
            {
                query = query.Where(u =>
                    u.Email.StartsWith(request.Text) ||
                    u.FirstName.StartsWith(request.Text) ||
                    u.LastName.StartsWith(request.Text));
            }

            return await query
                .OrderBy(request)
                .ProjectToPageAsync<GetUsersReadModel>(mapper.ConfigurationProvider, request.PageIndex, request.PageSize);
        }
    }
}

