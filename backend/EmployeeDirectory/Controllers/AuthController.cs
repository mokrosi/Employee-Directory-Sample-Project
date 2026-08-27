using Microsoft.AspNetCore.Mvc;
using MediatR;
using EmployeeDirectory.Application.Features.Auth.Commands.RegisterUser;
using System.Threading.Tasks;

namespace EmployeeDirectory.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        try
        {
            var userId = await _mediator.Send(command);

            return Ok(new { message = "user registered successfully", userId = userId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}