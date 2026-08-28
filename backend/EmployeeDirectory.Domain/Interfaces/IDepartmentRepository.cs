using EmployeeDirectory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeDirectory.Domain.Interfaces
{
    public interface IDepartmentRepository
    {

        Task<Department> AddAsync(Department department);
        Task<bool> IsNameUniqueAsync(string name);
        Task<Department?> GetByIdAsync(Guid id);
        Task<IEnumerable<Department>> GetAllAsync();
    }
}
