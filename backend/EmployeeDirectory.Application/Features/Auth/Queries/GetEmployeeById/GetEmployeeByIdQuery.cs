using MediatR;
using System;
using System.Collections.Generic;

namespace EmployeeDirectory.Application.Features.Employees.Queries.GetEmployeeById;

public class DepartmentHistoryDto
{
    public string DepartmentName { get; set; } = string.Empty;
    public DateTime TransferredAt { get; set; }
}

public class EmployeeDetailDto
{
    public Guid Id { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CurrentDepartment { get; set; } = string.Empty;

    public List<DepartmentHistoryDto> History { get; set; } = new();
}

public class GetEmployeeByIdQuery : IRequest<EmployeeDetailDto>
{
    public Guid EmployeeId { get; set; }
}