using EmployeeDirectory.Application.Interfaces;
using EmployeeDirectory.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeDirectory.Application.Features.Auth.Queries.LoginUser;

public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, string?>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtProvider;

    public LoginUserQueryHandler(IUserRepository userRepository, IJwtTokenGenerator jwtProvider)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<string?> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email.Trim());

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return null;
        }

        return _jwtProvider.GenerateToken(user);
    }
}