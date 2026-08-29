using EmployeeDirectory.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeDirectory.Application.Features.Departments.Commands.UpdateDepartment;

public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, bool>
{
    private readonly IDepartmentRepository _departmentRepository;

    public UpdateDepartmentCommandHandler(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<bool> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await _departmentRepository.GetByIdAsync(request.Id);
        if (department == null)
            throw new Exception("Department not found.");

        var isUnique = await _departmentRepository.IsNameUniqueAsync(request.Name, request.Id);
        if (!isUnique)
            throw new Exception("Department name is already in use.");

        department.Name = request.Name.Trim();
        department.MaxHeadcount = request.MaxHeadcount;

        await _departmentRepository.UpdateAsync(department);
        return true;
    }
}
