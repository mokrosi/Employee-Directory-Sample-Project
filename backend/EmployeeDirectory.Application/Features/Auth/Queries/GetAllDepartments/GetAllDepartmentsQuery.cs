using MediatR;
using System;
using System.Collections.Generic;

namespace EmployeeDirectory.Application.Features.Departments.Queries.GetAllDepartments;

public class DepartmentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int MaxHeadcount { get; set; }
    public int CurrentHeadcount { get; set; }
}

public class GetAllDepartmentsQuery : IRequest<List<DepartmentDto>>
{
}