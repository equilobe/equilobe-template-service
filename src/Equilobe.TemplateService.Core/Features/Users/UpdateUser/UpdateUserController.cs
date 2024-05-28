
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Equilobe.TemplateService.Core.Features.Users.UpdateUser;

public class UpdateUserController : ControllerBase
{
    private readonly IMediator mediator;

    public UpdateUserController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPut("api/users/{id}")]
    [UserSwaggerOperation]
    public async Task<IActionResult> UpdateUser([FromRoute] long id, [FromBody] UpdateUserCommand command)
    {
        command.Id = id;
        await mediator.Send(command);

        return Ok();
    }
}