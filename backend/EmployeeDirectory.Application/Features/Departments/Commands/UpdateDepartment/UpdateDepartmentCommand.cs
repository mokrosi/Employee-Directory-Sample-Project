using MediatR;
using System;

namespace EmployeeDirectory.Application.Features.Departments.Commands.UpdateDepartment;

public class UpdateDepartmentCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int MaxHeadcount { get; set; }
}
