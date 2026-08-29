using EmployeeDirectory.Domain.Interfaces;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeDirectory.Application.Features.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    private readonly IEmployeeRepository _employeeRepository;

    public CreateEmployeeCommandValidator(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;

        RuleFor(p => p.EmployeeCode)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(10).WithMessage("Employee code must not exceed 10 characters.")
            .MustAsync(BeUniqueEmployeeCode).WithMessage("Employee code is already in use.");

        RuleFor(p => p.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(100).WithMessage("Full name must not exceed 100 characters.");

        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MustAsync(BeUniqueEmail).WithMessage("Employee email is already in use.");
    }

    private async Task<bool> BeUniqueEmployeeCode(string code, CancellationToken cancellationToken)
    {
        return await _employeeRepository.IsEmployeeCodeUniqueAsync(code);
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        return await _employeeRepository.IsEmailUniqueAsync(email);
    }
}