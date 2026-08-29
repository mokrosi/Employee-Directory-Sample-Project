using EmployeeDirectory.Application.Features.Auth.Commands.CreateDepartment;
using EmployeeDirectory.Domain.Interfaces;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeDirectory.Application.Features.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    private readonly IDepartmentRepository _departmentRepository;

    public CreateDepartmentCommandValidator(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;

        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Department name is required.")
            .MaximumLength(100).WithMessage("Department name must not exceed 100 characters.")
            .MustAsync(BeUniqueName).WithMessage("Department name is already in use.");

        RuleFor(p => p.MaxHeadcount)
            .GreaterThan(0).WithMessage("Maximum headcount must be at least 1.");
    }

    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _departmentRepository.IsNameUniqueAsync(name);
    }
}
