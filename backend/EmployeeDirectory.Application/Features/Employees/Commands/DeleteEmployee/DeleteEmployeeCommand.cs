using MediatR;
using System;

namespace EmployeeDirectory.Application.Features.Employees.Commands.DeleteEmployee;

public class DeleteEmployeeCommand : IRequest<bool>
{
    public Guid EmployeeId { get; set; }
}