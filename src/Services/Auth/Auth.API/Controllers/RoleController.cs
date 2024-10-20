using Auth.Application.DTOs.Request.Role;
using Auth.Application.UseCases.V1.Commands.Role.AssignRole;
using Auth.Application.UseCases.V1.Commands.Role.CreateRole;
using Auth.Application.UseCases.V1.Commands.Role.DeleteRole;
using Auth.Application.UseCases.V1.Commands.Role.UpdateRole;
using Auth.Application.UseCases.V1.Queries.Role.GetRoleByUser;
using Auth.Application.UseCases.V1.Queries.Role.GetRoles;
using Auth.Application.UseCases.V1.Queries.Role.GetUserByRole;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Auth.API.Controllers;

[ApiController]
[Route("/api/v1/roles")]
public class RoleController(IMediator mediator): Controller
{
    

    [HttpPost()]
    public async Task<IActionResult> Create(CreateRoleRequestDto createRoleRequestDto)
    {
        var response = await mediator.Send(new CreateRoleCommand(CreateRoleRequestDto: createRoleRequestDto));
        return Ok(response);
    }
    
    
    [HttpPatch()]
    public async Task<IActionResult> Update(UpdateRoleRequestDto updateRoleRequestDto)
    {
        var response = await mediator.Send(new UpdateRoleCommand(UpdateRoleRequestDto: updateRoleRequestDto));
        return Ok(response);
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole(AssignRoleRequestDto assignRoleRequestDto)
    {
        var response = await mediator.Send(new AssignRoleCommand(AssignRoleRequestDto: assignRoleRequestDto));
        return Ok(response);
    }
    
    [HttpDelete("{roleName}")]
    public async Task<IActionResult> Delete(string roleName)
    {
        var response = await mediator.Send(new DeleteRoleCommand(roleName));
        return Ok(response);
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var response = await mediator.Send(new GetRolesQuery());
        return Ok(response);
    }


    [HttpGet("get-role-by-user/{userId}")]
    public async Task<IActionResult> GetRoleByUser(string userId)
    {
        var response = await mediator.Send(new GetRoleByUserQuery(userId));
        return Ok(response);
    }
    
    
    [HttpGet("get-user-by-role/{roleName}")]
    public async Task<IActionResult> GetUserByRole(string roleName)
    {
        var response = await mediator.Send(new GetUserByRoleQuery(roleName));
        return Ok(response);
    }
    
    
    
}