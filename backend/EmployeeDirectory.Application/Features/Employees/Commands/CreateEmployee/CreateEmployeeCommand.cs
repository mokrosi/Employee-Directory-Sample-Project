using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using System;

namespace EmployeeDirectory.Application.Features.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommand : IRequest<Guid>
{
    public string EmployeeCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
}