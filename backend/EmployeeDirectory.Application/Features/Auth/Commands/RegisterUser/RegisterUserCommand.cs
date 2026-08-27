using System;
using System.Collections.Generic;
using System.Text;
using System;
using MediatR;

namespace EmployeeDirectory.Application.Features.Auth.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<Guid>
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}