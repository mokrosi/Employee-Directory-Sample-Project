using MediatR;
using System;

namespace EmployeeDirectory.Application.Features.Employees.Commands.TransferEmployee;

public class TransferEmployeeCommand : IRequest<bool>
{
    public Guid EmployeeId { get; set; }
    public Guid NewDepartmentId { get; set; }
}