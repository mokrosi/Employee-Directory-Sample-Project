using EmployeeDirectory.Application.Features.Auth.Commands.CreateDepartment;
using EmployeeDirectory.Domain.Entities;
using EmployeeDirectory.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeDirectory.Application.Features.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, Guid>
{
    private readonly IDepartmentRepository _departmentRepository;

    public CreateDepartmentCommandHandler(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<Guid> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var isUnique = await _departmentRepository.IsNameUniqueAsync(request.Name);
        if (!isUnique)
        {
            throw new Exception("Department name is already in use.");
        }

        var department = new Department
        {
            Name = request.Name,
            MaxHeadcount = request.MaxHeadcount
        };

        var createdDepartment = await _departmentRepository.AddAsync(department);

        return createdDepartment.Id;
    }
}