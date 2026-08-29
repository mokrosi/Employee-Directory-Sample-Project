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
[Route("api/departments")]
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
        try
        {
            var departmentId = await _mediator.Send(command);
            return Ok(new { message = "department created successfully", departmentId = departmentId });
        }
        catch (FluentValidation.ValidationException ex)
        {
            return BadRequest(new { message = ex.Errors.FirstOrDefault()?.ErrorMessage ?? ex.Message, errors = ex.Errors });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDepartments()
    {
        try
        {
            var query = new GetAllDepartmentsQuery();
            var departments = await _mediator.Send(query);
            return Ok(departments);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDepartment(Guid id, [FromBody] EmployeeDirectory.Application.Features.Departments.Commands.UpdateDepartment.UpdateDepartmentCommand command)
    {
        try
        {
            command.Id = id;
            await _mediator.Send(command);
            return Ok(new { message = "Department updated successfully." });
        }
        catch (FluentValidation.ValidationException ex)
        {
            return BadRequest(new { message = ex.Errors.FirstOrDefault()?.ErrorMessage ?? ex.Message, errors = ex.Errors });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}