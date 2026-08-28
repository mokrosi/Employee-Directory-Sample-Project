using MediatR;
using System;

namespace EmployeeDirectory.Application.Features.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}