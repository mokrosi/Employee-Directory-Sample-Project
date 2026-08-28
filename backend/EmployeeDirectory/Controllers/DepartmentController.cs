using EmployeeDirectory.Application.Features.Auth.Commands.CreateDepartment;
using EmployeeDirectory.Application.Features.Departments.Commands.CreateDepartment;
using EmployeeDirectory.Application.Features.Departments.Queries.GetAllDepartments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EmployeeDirectory.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DepartmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public DepartmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentCommand command)
    {


            var departmentId = await _mediator.Send(command);

            return Ok(new { message = "department created successfully", departmentId = departmentId });

    }

    [HttpGet]
    public async Task<IActionResult> GetAllDepartments()
    {

            var query = new GetAllDepartmentsQuery();
            var departments = await _mediator.Send(query);
            return Ok(departments);

    }
}