using System;
using System.Collections.Generic;
using System.Text;
using EmployeeDirectory.Domain.Entities;

namespace EmployeeDirectory.Domain.Interfaces;

public interface IUserRepository
{
    Task<bool> IsEmailUniqueAsync(string email);

    Task<User> AddAsync(User user);

}