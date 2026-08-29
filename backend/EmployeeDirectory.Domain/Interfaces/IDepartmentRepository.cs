using EmployeeDirectory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeDirectory.Domain.Interfaces
{
    public interface IDepartmentRepository
    {

        Task<Department> AddAsync(Department department);
        Task<bool> IsNameUniqueAsync(string name, Guid? excludeDepartmentId = null);
        Task<Department?> GetByIdAsync(Guid id);
        Task<IEnumerable<Department>> GetAllAsync();
        Task UpdateAsync(Department department);
    }
}
