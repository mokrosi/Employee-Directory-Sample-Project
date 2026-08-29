using MediatR;
using System;
using System.Collections.Generic;

namespace EmployeeDirectory.Application.Features.Employees.Queries.GetEmployeeById;

public class DepartmentHistoryDto
{
    public Guid Id { get; set; }
    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public Guid TransferredByUserId { get; set; }
    public DateTime TransferredAt { get; set; }
}

public class EmployeeDetailDto
{
    public Guid Id { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string CurrentDepartment
    {
        get => DepartmentName;
        set => DepartmentName = value;
    }

    public List<DepartmentHistoryDto> History { get; set; } = new();
}

public class GetEmployeeByIdQuery : IRequest<EmployeeDetailDto>
{
    public Guid EmployeeId { get; set; }
}