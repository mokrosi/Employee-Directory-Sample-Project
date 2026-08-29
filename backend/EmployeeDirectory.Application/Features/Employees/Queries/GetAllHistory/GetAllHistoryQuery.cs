using MediatR;
using System;
using System.Collections.Generic;

namespace EmployeeDirectory.Application.Features.Employees.Queries.GetAllHistory;

public class HistoryRecordDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string EmployeeName { get; set; } = string.Empty;
    public string EmployeeEmail { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public Guid TransferredByUserId { get; set; }
    public string TransferredByName { get; set; } = string.Empty;
    public string TransferredByEmail { get; set; } = string.Empty;
    public DateTime TransferredAt { get; set; }
}

public class GetAllHistoryQuery : IRequest<List<HistoryRecordDto>>
{
}
