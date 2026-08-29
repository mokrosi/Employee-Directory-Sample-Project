using EmployeeDirectory.Application.Features.Employees.Commands.CreateEmployee;
using EmployeeDirectory.Application.Features.Employees.Commands.DeleteEmployee;
using EmployeeDirectory.Application.Features.Employees.Commands.TransferEmployee;
using EmployeeDirectory.Application.Features.Employees.Commands.UpdateEmployee;
using EmployeeDirectory.Application.Features.Employees.Queries.GetEmployeeById;
using EmployeeDirectory.Application.Features.Employees.Queries.SearchEmployees;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EmployeeDirectory.Controllers;

[Route("api/[controller]")]
[Route("api/employees")]
[ApiController]
[Authorize]
public class EmployeeController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeCommand command)
    {
        try
        {
            var employeeId = await _mediator.Send(command);
            return Ok(new { message = "Employee created successfully", employeeId = employeeId });
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
    [HttpGet("search")]
    public async Task<IActionResult> SearchEmployees(
        [FromQuery] string search = "",
        [FromQuery] string searchTerm = "",
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var query = new SearchEmployeesQuery
            {
                SearchTerm = !string.IsNullOrWhiteSpace(search) ? search : (searchTerm ?? string.Empty),
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> TransferEmployee([FromBody] TransferEmployeeCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Ok(new { message = "Employee transferred successfully." });
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

    [HttpGet("all-history")]
    [HttpGet("/api/history")]
    public async Task<IActionResult> GetAllHistory()
    {
        try
        {
            var query = new EmployeeDirectory.Application.Features.Employees.Queries.GetAllHistory.GetAllHistoryQuery();
            var histories = await _mediator.Send(query);
            return Ok(histories);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetEmployeeById(Guid id)
    {
        try
        {
            var query = new GetEmployeeByIdQuery { EmployeeId = id };
            var employee = await _mediator.Send(query);
            if (employee == null) return NotFound();
            return Ok(employee);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{id:guid}/history")]
    public async Task<IActionResult> GetEmployeeHistory(Guid id)
    {
        try
        {
            var query = new GetEmployeeByIdQuery { EmployeeId = id };
            var employee = await _mediator.Send(query);
            if (employee == null) return NotFound();
            return Ok(employee.History);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEmployee(Guid id)
    {
        try
        {
            var command = new DeleteEmployeeCommand { EmployeeId = id };
            await _mediator.Send(command);
            return Ok(new { message = "Employee deleted successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] UpdateEmployeeCommand command)
    {
        try
        {
            command.Id = id;
            await _mediator.Send(command);
            return Ok(new { message = "Employee updated successfully." });
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