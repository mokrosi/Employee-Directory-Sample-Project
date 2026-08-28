using FluentValidation;

namespace EmployeeDirectory.Application.Features.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(p => p.EmployeeCode)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(10).WithMessage("Employee code must not exceed 10 characters.");

        RuleFor(p => p.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(100).WithMessage("Full name must not exceed 100 characters.");

        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}