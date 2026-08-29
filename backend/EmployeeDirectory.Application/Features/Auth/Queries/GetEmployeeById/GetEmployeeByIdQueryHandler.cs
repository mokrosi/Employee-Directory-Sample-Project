using EmployeeDirectory.Domain.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeDirectory.Application.Features.Employees.Queries.GetEmployeeById;

public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDetailDto>
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<EmployeeDetailDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);

        if (employee == null)
        {
            throw new Exception("Employee not found.");
        }

        return new EmployeeDetailDto
        {
            Id = employee.Id,
            EmployeeCode = employee.EmployeeCode,
            FullName = employee.FullName,
            Email = employee.Email,
            DepartmentId = employee.DepartmentId,
            DepartmentName = employee.Department?.Name ?? "No Department",
            History = employee.DepartmentHistories.Select(h => new DepartmentHistoryDto
            {
                Id = h.Id,
                DepartmentId = h.DepartmentId,
                DepartmentName = h.Department?.Name ?? "Unknown",
                TransferredByUserId = h.TransferredByUserId,
                TransferredAt = h.TransferredAt
            }).OrderByDescending(h => h.TransferredAt).ToList()
        };
    }
}