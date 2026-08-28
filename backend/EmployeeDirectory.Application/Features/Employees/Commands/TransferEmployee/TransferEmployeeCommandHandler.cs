using EmployeeDirectory.Application.Interfaces;
using EmployeeDirectory.Domain.Entities;
using EmployeeDirectory.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeDirectory.Application.Features.Employees.Commands.TransferEmployee;

public class TransferEmployeeCommandHandler : IRequestHandler<TransferEmployeeCommand, bool>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public TransferEmployeeCommandHandler(
        IEmployeeRepository employeeRepository,
        IDepartmentRepository departmentRepository,
        ICurrentUserService currentUserService)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(TransferEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
        if (employee == null) throw new Exception("Employee not found.");

        if (employee.DepartmentId == request.NewDepartmentId)
            throw new Exception("Employee is already in the requested department.");

        var newDepartment = await _departmentRepository.GetByIdAsync(request.NewDepartmentId);
        if (newDepartment == null) throw new Exception("New department not found.");

        var currentHeadcount = await _employeeRepository.GetCountByDepartmentIdAsync(request.NewDepartmentId);
        if (currentHeadcount >= newDepartment.MaxHeadcount)
            throw new Exception($"Sorry, the new department has reached its maximum capacity ({newDepartment.MaxHeadcount}). The employee cannot be transferred there.");

        employee.DepartmentId = request.NewDepartmentId;

        employee.DepartmentHistories.Add(new EmployeeDepartmentHistory
        {
            Id = Guid.NewGuid(),
            DepartmentId = request.NewDepartmentId,
            TransferredByUserId = _currentUserService.UserId,
            TransferredAt = DateTime.UtcNow
        });

        await _employeeRepository.UpdateAsync(employee);
        return true;
    }
}