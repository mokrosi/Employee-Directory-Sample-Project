using EmployeeDirectory.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeDirectory.Application.Features.Employees.Queries.GetAllHistory;

public class GetAllHistoryQueryHandler : IRequestHandler<GetAllHistoryQuery, List<HistoryRecordDto>>
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetAllHistoryQueryHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<List<HistoryRecordDto>> Handle(GetAllHistoryQuery request, CancellationToken cancellationToken)
    {
        var histories = await _employeeRepository.GetAllHistoryAsync();

        return histories.Select(h => new HistoryRecordDto
        {
            Id = h.Id,
            EmployeeId = h.EmployeeId,
            EmployeeCode = h.Employee?.EmployeeCode ?? "N/A",
            EmployeeName = h.Employee?.FullName ?? "Unknown",
            EmployeeEmail = h.Employee?.Email ?? "N/A",
            DepartmentId = h.DepartmentId,
            DepartmentName = h.Department?.Name ?? "Unknown",
            TransferredByUserId = h.TransferredByUserId,
            TransferredByName = h.TransferredByUser?.FullName ?? "System Administrator",
            TransferredByEmail = h.TransferredByUser?.Email ?? "admin@system.debug",
            TransferredAt = h.TransferredAt
        }).ToList();
    }
}
