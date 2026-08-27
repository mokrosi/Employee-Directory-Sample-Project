using System;
using System.Threading;
using System.Threading.Tasks;
using EmployeeDirectory.Application.Interfaces;
using EmployeeDirectory.Domain.Interfaces;
using MediatR;

namespace EmployeeDirectory.Application.Features.Auth.Queries.LoginUser;

public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginUserQueryHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<string> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null)
        {
            throw new Exception("Email or password is incorrect.");
        }

        bool isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            throw new Exception("Email or password is incorrect.");
        }

        string token = _jwtTokenGenerator.GenerateToken(user);

        return token;
    }
}