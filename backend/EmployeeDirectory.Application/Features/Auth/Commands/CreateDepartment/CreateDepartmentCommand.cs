using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeDirectory.Application.Features.Auth.Commands.CreateDepartment
{
    public class CreateDepartmentCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public int MaxHeadcount { get; set; }
    }
}
