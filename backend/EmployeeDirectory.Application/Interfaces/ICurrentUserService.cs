using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeDirectory.Application.Interfaces;

public interface ICurrentUserService
{
    Guid UserId { get; }
}