using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeDirectory.Application.Features.Auth.Queries.LoginUser
{
    public class LoginUserQuery : IRequest<string>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
