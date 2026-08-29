using EmployeeDirectory.Domain.Interfaces;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeDirectory.Application.Features.Departments.Commands.UpdateDepartment;

public class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
{
    private readonly IDepartmentRepository _departmentRepository;

    public UpdateDepartmentCommandValidator(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;

        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("Department ID is required.");

        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Department name is required.")
            .MaximumLength(100).WithMessage("Department name must not exceed 100 characters.");

        RuleFor(p => p.MaxHeadcount)
            .GreaterThan(0).WithMessage("Max headcount must be greater than 0.");

        RuleFor(p => p)
            .MustAsync(BeUniqueName).WithMessage("Department name is already in use.");
    }

    private async Task<bool> BeUniqueName(UpdateDepartmentCommand command, CancellationToken cancellationToken)
    {
        return await _departmentRepository.IsNameUniqueAsync(command.Name, command.Id);
    }
}
