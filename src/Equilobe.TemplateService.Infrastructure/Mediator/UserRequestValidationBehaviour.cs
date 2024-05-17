using System;
using Equilobe.TemplateService.Infrastructure.Database;
using FluentValidation;
using MediatR;
using Equilobe.TemplateService.Infrastructure.Database.Extensions;
using Equilobe.TemplateService.Core.Common.Exceptions;

namespace Equilobe.TemplateService.Infrastructure.Mediator
{
    public class UserRequestValidationBehaviour<TRequest, TResponse>(
        AppDbContext dbContext) :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var isUserRequest = request
            .GetType()
            .GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IUserRequest<>));

            if (!isUserRequest)
                return await next();

            var userRequest = (IUserRequest<TResponse>)request;
            var user = await _dbContext.GetUserOrDefaultAsync(userRequest.UserId) ?? throw BadRequestException.Create($"User {userRequest.UserId} does not exist");
            
            return await next();
        }
    }

}

