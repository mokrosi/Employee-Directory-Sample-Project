using EmployeeDirectory.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeDirectory.Application.Features.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, bool>
{
    private readonly IEmployeeRepository _employeeRepository;

    public UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<bool> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.Id);

        if (employee == null)
            throw new Exception("Employee not found");

        var isEmailUnique = await _employeeRepository.IsEmailUniqueAsync(request.Email, request.Id);
        if (!isEmailUnique)
            throw new Exception("Employee email is already in use.");

        employee.FullName = request.FullName;
        employee.Email = request.Email;

        await _employeeRepository.UpdateAsync(employee);

        return true;
    }
}