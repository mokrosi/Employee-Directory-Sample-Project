using System;
using System.Threading;
using System.Threading.Tasks;
using EmployeeDirectory.Application.Interfaces;
using EmployeeDirectory.Domain.Entities;
using EmployeeDirectory.Domain.Interfaces;
using MediatR;

namespace EmployeeDirectory.Application.Features.Auth.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        bool isUnique = await _userRepository.IsEmailUniqueAsync(request.Email);
        if (!isUnique)
        {
            throw new Exception("Email is already registered ");
        }

        string hashedPassword = _passwordHasher.HashPassword(request.Password);

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = hashedPassword,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(newUser);

        return newUser.Id;
    }
}