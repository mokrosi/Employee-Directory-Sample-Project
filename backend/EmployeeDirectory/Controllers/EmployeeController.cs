using EmployeeDirectory.Application.Features.Employees.Commands.CreateEmployee;
using EmployeeDirectory.Application.Features.Employees.Commands.DeleteEmployee;
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

            var employeeId = await _mediator.Send(command);
            return Ok(new { message = "Employee created successfully", employeeId = employeeId });

    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchEmployees(
        [FromQuery] string searchTerm = "",
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {

            var query = new SearchEmployeesQuery
            {
                SearchTerm = searchTerm ?? string.Empty,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result);

    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeById(Guid id)
    {

            var query = new GetEmployeeByIdQuery { EmployeeId = id };
            var employee = await _mediator.Send(query);
            return Ok(employee);
        

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(Guid id)
    {

            var command = new DeleteEmployeeCommand { EmployeeId = id };
            await _mediator.Send(command);
            return Ok(new { message = "Employee deleted successfully." });

    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] UpdateEmployeeCommand command)
    {

            command.Id = id;

            await _mediator.Send(command);
            return Ok(new { message = "Employee updated successfully." });

    }
}