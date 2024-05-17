using Equilobe.TemplateService.Core.Common.Exceptions;
using Equilobe.TemplateService.Core.Common.Interfaces;
using Equilobe.TemplateService.Infrastructure.Services;
using MediatR;

namespace Equilobe.TemplateService.Infrastructure.Mediator.Extensions;

public class UserParametersBehaviour<TRequest, TResponse>(
    IUserProvider userProvider) :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IUserProvider _userProvider = userProvider;

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

        var userRequest = (IUserRequest<TResponse>)request ?? throw new AuthorizationException();
        var userId = await _userProvider.GetCurrentUserIdOrDefaultAsync() ?? throw new AuthorizationException();

        userRequest.UserId = userId;

        return await next();
    }
}