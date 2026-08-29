using EmployeeDirectory.Application.Interfaces;
using EmployeeDirectory.Domain.Entities;
using EmployeeDirectory.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeDirectory.Application.Features.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Guid>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateEmployeeCommandHandler(
        IEmployeeRepository employeeRepository,
        IDepartmentRepository departmentRepository,
        ICurrentUserService currentUserService)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var isUnique = await _employeeRepository.IsEmployeeCodeUniqueAsync(request.EmployeeCode);
        if (!isUnique)
            throw new Exception("Employee code is already in use.");

        var isEmailUnique = await _employeeRepository.IsEmailUniqueAsync(request.Email);
        if (!isEmailUnique)
            throw new Exception("Employee email is already in use.");

        var department = await _departmentRepository.GetByIdAsync(request.DepartmentId);
        if (department == null)
            throw new Exception("Selected department does not exist.");

        var currentHeadcount = await _employeeRepository.GetCountByDepartmentIdAsync(request.DepartmentId);
        if (currentHeadcount >= department.MaxHeadcount)
            throw new Exception($"Sorry, the department has reached its maximum capacity ({department.MaxHeadcount}). No new employees can be added.");

        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            EmployeeCode = request.EmployeeCode,
            FullName = request.FullName,
            Email = request.Email,
            DepartmentId = request.DepartmentId,
            CreatedByUserId = _currentUserService.UserId,
            CreatedAt = DateTime.UtcNow,
            DepartmentHistories = new List<EmployeeDepartmentHistory>()
        };

        employee.DepartmentHistories.Add(new EmployeeDepartmentHistory
        {
            Id = Guid.NewGuid(),
            DepartmentId = request.DepartmentId,
            TransferredByUserId = _currentUserService.UserId,
            TransferredAt = DateTime.UtcNow
        });

        await _employeeRepository.AddAsync(employee);

        return employee.Id;
    }
}