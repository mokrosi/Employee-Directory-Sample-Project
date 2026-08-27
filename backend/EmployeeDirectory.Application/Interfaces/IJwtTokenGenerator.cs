using System;
using System.Collections.Generic;
using System.Text;
using EmployeeDirectory.Domain.Entities;

namespace EmployeeDirectory.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}