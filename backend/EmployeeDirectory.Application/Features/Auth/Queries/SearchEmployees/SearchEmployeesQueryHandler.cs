using EmployeeDirectory.Domain.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeDirectory.Application.Features.Employees.Queries.SearchEmployees;

public class SearchEmployeesQueryHandler : IRequestHandler<SearchEmployeesQuery, PaginatedEmployeeResult>
{
    private readonly IEmployeeRepository _employeeRepository;

    public SearchEmployeesQueryHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<PaginatedEmployeeResult> Handle(SearchEmployeesQuery request, CancellationToken cancellationToken)
    {
        var (employees, totalCount) = await _employeeRepository.SearchAsync(
            request.SearchTerm,
            request.PageNumber,
            request.PageSize);

        var items = employees.Select(e => new EmployeeDto
        {
            Id = e.Id,
            EmployeeCode = e.EmployeeCode,
            FullName = e.FullName,
            Email = e.Email,
            DepartmentId = e.DepartmentId,
            DepartmentName = e.Department?.Name ?? "No department",
            CreatedAt = e.CreatedAt
        }).ToList();

        return new PaginatedEmployeeResult
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}