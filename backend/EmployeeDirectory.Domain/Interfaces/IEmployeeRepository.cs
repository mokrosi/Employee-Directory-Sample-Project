using EmployeeDirectory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeDirectory.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee> AddAsync(Employee employee);
        Task<bool> IsEmployeeCodeUniqueAsync(string employeeCode);
        Task<int> GetCountByDepartmentIdAsync(Guid departmentId);
        Task<(IEnumerable<Employee> Employees, int TotalCount)> SearchAsync(string searchTerm, int pageNumber, int pageSize);
        Task<Employee?> GetByIdAsync(Guid id);

        Task UpdateAsync(Employee employee);
        Task DeleteAsync(Employee employee);

        
    }
}
