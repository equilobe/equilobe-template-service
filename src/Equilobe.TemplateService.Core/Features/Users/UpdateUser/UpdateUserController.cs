
using AutoMapper;
using Equilobe.TemplateService.Core.Common.Exceptions;
using Equilobe.TemplateService.Core.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemTextJsonPatch;

namespace Equilobe.TemplateService.Core.Features.Users.UpdateUser;

[Route("api/users")]
public class UpdateUserController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IDbContext dbContext;

    public UpdateUserController(IMediator mediator, IMapper mapper, IDbContext dbContext)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.dbContext = dbContext;
    }

    [HttpPut("{id}")]
    [UserSwaggerOperation]
    public async Task<IActionResult> UpdateUser([FromRoute] long id, [FromBody] UpdateUserCommand command)
    {
        command.Id = id;
        await mediator.Send(command);

        return Ok();
    }

    [HttpPatch("{id}")]
    [UserSwaggerOperation]
    public async Task<IActionResult> PatchUser([FromBody] JsonPatchDocument<UpdateUserCommand> patchRequest, [FromRoute] long id)
    {
        var userFromDb = await dbContext.Users.SingleOrDefaultAsync(u =>  u.Id == id);
        if (userFromDb is null)
        {
            throw new ResourceNotFoundException("User not found.");
        }

        var currentUpdateUserCommand = mapper.Map<UpdateUserCommand>(userFromDb);
        patchRequest.ApplyTo(currentUpdateUserCommand);

        currentUpdateUserCommand.Id = id;
        await mediator.Send(currentUpdateUserCommand);

        return Ok();
    }
}
