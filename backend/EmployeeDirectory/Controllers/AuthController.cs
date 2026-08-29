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
        try
        {
            var userId = await _mediator.Send(command);
            return Ok(new { message = "Registration successful", userId });
        }
        catch (FluentValidation.ValidationException ex)
        {
            return BadRequest(new { message = ex.Errors.FirstOrDefault()?.ErrorMessage ?? ex.Message, errors = ex.Errors });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserQuery query)
    {
        var token = await _mediator.Send(query);
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized(new { message = "Invalid email or password." });
        }
        return Ok(new { token });
    }

}