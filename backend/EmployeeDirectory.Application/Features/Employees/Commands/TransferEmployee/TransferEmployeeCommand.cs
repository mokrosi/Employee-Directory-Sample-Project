using MediatR;
using System;
using System.Text.Json.Serialization;

namespace EmployeeDirectory.Application.Features.Employees.Commands.TransferEmployee;

public class TransferEmployeeCommand : IRequest<bool>
{
    public Guid EmployeeId { get; set; }
    
    public Guid NewDepartmentId { get; set; }

    [JsonPropertyName("targetDepartmentId")]
    public Guid? TargetDepartmentId
    {
        get => NewDepartmentId;
        set { if (value.HasValue && value.Value != Guid.Empty) NewDepartmentId = value.Value; }
    }
}