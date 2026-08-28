using Microsoft.AspNetCore.Mvc;
using MediatR;
using EmployeeDirectory.Application.Features.Auth.Commands.RegisterUser;
using System.Threading.Tasks;
using EmployeeDirectory.Application.Features.Auth.Queries.LoginUser;

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
        var userId = await _mediator.Send(command);
        return Ok(new { message = "Login successful", userId });
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserQuery query)
    {
        var token = await _mediator.Send(query);
        return Ok(new { token });
    }

}